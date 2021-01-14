using HateoasNet.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HateoasNet.Infrastructure
{
    /// <inheritdoc cref="IHateoasContext" />
    public sealed class HateoasContext : IHateoasContext
    {
        private static string ResourceBuilderTypeName => typeof(IHateoasSourceBuilder<>).Name;

        private readonly Dictionary<Type, IHateoasSource> _sources = new Dictionary<Type, IHateoasSource>();

        internal HateoasContext()
        {
        }

        public IEnumerable<IHateoasLinkBuilder> GetApplicableLinkBuilders(object source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return _sources.TryGetValue(source.GetType(), out var hateoasSource)
                ? hateoasSource?.GetLinkBuilders().Where(link => link.IsApplicable(source)).ToList()
                : new List<IHateoasLinkBuilder>();
        }

        public IHateoasContext Configure<T>(Action<IHateoasSource<T>> source) where T : class
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            source(GetOrInsert<T>());

            return this;
        }

        public IHateoasContext ApplyConfiguration<T>(IHateoasSourceBuilder<T> configuration) where T : class
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            configuration.Build(GetOrInsert<T>());

            return this;
        }

        public IHateoasContext ConfigureFromAssembly(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            var builders = assembly.GetTypes().Where(i => ImplementsHateoasSourceBuilder(i)).ToList();

            if (!builders.Any()) throw new TargetException(GetTargetExceptionMessage(assembly.FullName));

            builders.ForEach(builderType =>
            {
                var interfaceType = builderType.GetInterfaces().Single(IsHateoasSourceBuilder);
                var targetType = interfaceType.GetGenericArguments().First();
                var hateoasMap = GetOrInsert(targetType);
                var builder = Activator.CreateInstance(builderType);
                var buildMethod = builderType.GetMethod(nameof(IHateoasSourceBuilder<object>.Build));
                buildMethod?.Invoke(builder, new object[] { hateoasMap });
            });

            return this;
        }

        internal IHateoasSource<T> GetOrInsert<T>() where T : class
        {
            var targetType = typeof(T);

            if (!_sources.ContainsKey(targetType)) _sources.Add(targetType, new HateoasSource<T>());

            return _sources[targetType] as IHateoasSource<T>;
        }

        internal IHateoasSource GetOrInsert(Type targetType)
        {
            if (_sources.ContainsKey(targetType)) return _sources[targetType];

            var hateoasSourceType = typeof(HateoasSource<>).MakeGenericType(targetType);
            _sources.Add(targetType, Activator.CreateInstance(hateoasSourceType, true) as IHateoasSource);

            return _sources[targetType];
        }

        private static bool ImplementsHateoasSourceBuilder(Type type)
        {
            return type.GetInterfaces().Any(i => IsHateoasSourceBuilder(i));
        }

        private static bool IsHateoasSourceBuilder(Type type)
        {
            return type.IsInterface && type.Name.Contains(ResourceBuilderTypeName);
        }

        private static string GetTargetExceptionMessage(string assemblyName)
        {
            return $"No implementation of '{ResourceBuilderTypeName}' found in assembly '{assemblyName}'.";
        }
    }
}
