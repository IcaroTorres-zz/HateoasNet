using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HateoasNet.Abstractions;

namespace HateoasNet.Mapping
{
	public class HateoasConfiguration : IHateoasConfiguration
	{
		private readonly Dictionary<Type, IHateoasMap> _maps = new Dictionary<Type, IHateoasMap>();

		public IEnumerable<IHateoasLink> GetMappedLinks(Type sourceType, object resourceData)
		{
			if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));

			if (resourceData == null) throw new ArgumentNullException(nameof(resourceData));

			return _maps[sourceType].GetLinks().Where(link => link.IsDisplayable(resourceData));
		}

		public bool HasMap(Type type)
		{
			return _maps.ContainsKey(type);
		}

		public IHateoasConfiguration Map<T>(Action<IHateoasMap<T>> mapper) where T : class
		{
			if (mapper == null) throw new ArgumentNullException(nameof(mapper));

			mapper(GetOrInsert<T>());

			return this;
		}

		public IHateoasConfiguration ApplyConfiguration<T>(IHateoasBuilder<T> builder) where T : class
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));

			builder.Build(GetOrInsert<T>());

			return this;
		}

		public IHateoasConfiguration ApplyConfigurationsFromAssembly(Assembly assembly)
		{
			if (assembly == null) throw new ArgumentNullException(nameof(assembly));

			var builders = assembly.GetTypes()
				.Where(t => t.GetInterfaces().Any(i => i.Name.Contains(typeof(IHateoasBuilder<>).Name))).ToList();

			if (!builders.Any())
				throw new TargetException($"No implementation of 'IHateoasBuilder' found in assembly '{assembly.FullName}'.");

			builders.ForEach(builderType =>
			{
				var interfaceType = builderType.GetInterfaces().Single();
				var targetType = interfaceType.GetGenericArguments().First();
				var hateoasMap = GetOrInsert(targetType);
				var builder = Activator.CreateInstance(builderType);
				var buildMethod = builderType.GetMethod(nameof(IHateoasBuilder<object>.Build));
				buildMethod.Invoke(builder, new object[] {hateoasMap});
			});

			return this;
		}

		internal IHateoasMap<T> GetOrInsert<T>() where T : class
		{
			var targetType = typeof(T);

			if (!_maps.ContainsKey(targetType)) _maps.Add(targetType, new HateoasMap<T>());

			return _maps[targetType] as IHateoasMap<T>;
		}

		internal IHateoasMap GetOrInsert(Type targetType)
		{
			if (_maps.ContainsKey(targetType)) return _maps[targetType];
			var hateoasMapType = typeof(HateoasMap<>).MakeGenericType(targetType);
			_maps.Add(targetType, Activator.CreateInstance(hateoasMapType) as IHateoasMap);

			return _maps[targetType];
		}
	}
}