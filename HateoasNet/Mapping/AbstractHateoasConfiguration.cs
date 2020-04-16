using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HateoasNet.Abstractions;

namespace HateoasNet.Mapping
{
	public abstract class AbstractHateoasConfiguration : IHateoasConfiguration
	{
		protected readonly Dictionary<Type, IHateoasMap> Maps = new Dictionary<Type, IHateoasMap>();

		public virtual IEnumerable<IHateoasLink> GetMappedLinks(Type sourceType, object resourceData)
		{
			if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));

			if (resourceData == null) throw new ArgumentNullException(nameof(resourceData));

			return Maps[sourceType].GetLinks().Where(link => link.IsDisplayable(resourceData));
		}

		public virtual bool HasMap(Type type)
		{
			return Maps.ContainsKey(type);
		}

		public virtual IHateoasConfiguration Map<T>(Action<IHateoasMap<T>> mapper) where T : class
		{
			if (mapper == null) throw new ArgumentNullException(nameof(mapper));

			mapper(GetOrInsert<T>());

			return this;
		}

		public virtual IHateoasConfiguration ApplyConfiguration<T>(IHateoasBuilder<T> builder) where T : class
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));

			builder.Build(GetOrInsert<T>());

			return this;
		}

		public virtual IHateoasConfiguration ApplyConfigurationsFromAssembly(Assembly assembly)
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

		protected internal abstract IHateoasMap<T> GetOrInsert<T>() where T : class;
		protected internal abstract IHateoasMap GetOrInsert(Type targetType);
	}
}