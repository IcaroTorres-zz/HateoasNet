using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HateoasNet.Abstractions;

namespace HateoasNet.Configurations
{
	
	///<inheritdoc cref="IHateoasContext"/>
	public sealed class HateoasContext : IHateoasContext
	{
		private readonly Dictionary<Type, IHateoasResource> _maps = new Dictionary<Type, IHateoasResource>();

		public IEnumerable<IHateoasLink> GetApplicableLinks(Type type, object value)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));

			if (value == null) throw new ArgumentNullException(nameof(value));

			return _maps[type].GetLinks().Where(link => link.IsApplicable(value));
		}

		public bool HasSet(Type type)
		{
			return _maps.ContainsKey(type);
		}

		public IHateoasContext Configure<T>(Action<IHateoasResource<T>> resource) where T : class
		{
			if (resource == null) throw new ArgumentNullException(nameof(resource));

			resource(GetOrInsert<T>());

			return this;
		}

		public IHateoasContext ApplyConfiguration<T>(IHateoasResourceConfiguration<T> configuration) where T : class
		{
			if (configuration == null) throw new ArgumentNullException(nameof(configuration));

			configuration.Configure(GetOrInsert<T>());

			return this;
		}

		public IHateoasContext ApplyConfigurationsFromAssembly(Assembly assembly)
		{
			if (assembly == null) throw new ArgumentNullException(nameof(assembly));

			var builders = assembly.GetTypes()
				.Where(t => t.GetInterfaces().Any(i => i.Name.Contains(typeof(IHateoasResourceConfiguration<>).Name))).ToList();

			if (!builders.Any())
				throw new TargetException($"No implementation of 'IHateoasBuilder' found in assembly '{assembly.FullName}'.");

			builders.ForEach(builderType =>
			{
				var interfaceType = builderType.GetInterfaces().Single();
				var targetType = interfaceType.GetGenericArguments().First();
				var hateoasMap = GetOrInsert(targetType);
				var builder = Activator.CreateInstance(builderType);
				var buildMethod = builderType.GetMethod(nameof(IHateoasResourceConfiguration<object>.Configure));
				buildMethod?.Invoke(builder, new object[] {hateoasMap});
			});

			return this;
		}

		internal IHateoasResource<T> GetOrInsert<T>() where T : class
		{
			var targetType = typeof(T);

			if (!_maps.ContainsKey(targetType)) _maps.Add(targetType, new HateoasResource<T>());

			return _maps[targetType] as IHateoasResource<T>;
		}

		internal IHateoasResource GetOrInsert(Type targetType)
		{
			if (_maps.ContainsKey(targetType)) return _maps[targetType];

			var hateoasMapType = typeof(HateoasResource<>).MakeGenericType(targetType);
			_maps.Add(targetType, Activator.CreateInstance(hateoasMapType, true) as IHateoasResource);

			return _maps[targetType];
		}
	}
}