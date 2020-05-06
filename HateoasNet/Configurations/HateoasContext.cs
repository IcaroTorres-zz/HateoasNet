using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HateoasNet.Abstractions;

namespace HateoasNet.Configurations
{
	/// <inheritdoc cref="IHateoasContext" />
	public sealed class HateoasContext : IHateoasContext
	{
		private readonly string _resourceConfigurationTypeName = typeof(IHateoasResourceConfiguration<>).Name;

		private readonly Dictionary<Type, IHateoasResource> _resources = new Dictionary<Type, IHateoasResource>();

		internal HateoasContext()
		{
		}

		public IEnumerable<IHateoasLink> GetApplicableLinks(Type type, object value)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (value == null) throw new ArgumentNullException(nameof(value));

			return _resources.TryGetValue(type, out var configuredResource)
				? configuredResource?.GetLinks().Where(link => link.IsApplicable(value)).ToList()
				: new List<IHateoasLink>();
		}

		public bool HasResource(Type type)
		{
			return _resources.ContainsKey(type);
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

		public IHateoasContext ConfigureFromAssembly(Assembly assembly)
		{
			if (assembly == null) throw new ArgumentNullException(nameof(assembly));

			var builders = assembly.GetTypes().Where(ImplementsHateoasResourceConfiguration).ToList();

			if (!builders.Any()) throw new TargetException(GetTargetExceptionMessage(assembly.FullName));

			builders.ForEach(builderType =>
			{
				var interfaceType = builderType.GetInterfaces().Single(IsHateoasResourceConfiguration);
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

			if (!_resources.ContainsKey(targetType)) _resources.Add(targetType, new HateoasResource<T>());

			return _resources[targetType] as IHateoasResource<T>;
		}

		internal IHateoasResource GetOrInsert(Type targetType)
		{
			if (_resources.ContainsKey(targetType)) return _resources[targetType];

			var hateoasMapType = typeof(HateoasResource<>).MakeGenericType(targetType);
			_resources.Add(targetType, Activator.CreateInstance(hateoasMapType, true) as IHateoasResource);

			return _resources[targetType];
		}

		private bool ImplementsHateoasResourceConfiguration(Type type)
		{
			return type.GetInterfaces().Any(IsHateoasResourceConfiguration);
		}

		private bool IsHateoasResourceConfiguration(Type type)
		{
			return type.IsInterface && type.Name.Contains(_resourceConfigurationTypeName);
		}

		private string GetTargetExceptionMessage(string assemblyName)
		{
			return $"No implementation of '{_resourceConfigurationTypeName}' found in assembly '{assemblyName}'.";
		}
	}
}
