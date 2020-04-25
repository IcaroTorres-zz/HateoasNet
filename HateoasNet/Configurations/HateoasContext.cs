using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using HateoasNet.Abstractions;
using Microsoft.Win32.SafeHandles;

namespace HateoasNet.Configurations
{
	/// <inheritdoc cref="IHateoasContext" />
	public sealed class HateoasContext : IHateoasContext
	{
		private readonly SafeHandle _handle = new SafeFileHandle(IntPtr.Zero, true);
		private readonly Dictionary<Type, IHateoasResource> _resources = new Dictionary<Type, IHateoasResource>();
		private bool _disposed;

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

			var resourceConfigurationName = typeof(IHateoasResourceConfiguration<>).Name;

			var builders = assembly.GetTypes()
			                       .Where(t => t.GetInterfaces().Any(i => i.Name.Contains(resourceConfigurationName)))
			                       .ToList();

			if (!builders.Any())
				throw new
					TargetException($"No implementation of '{resourceConfigurationName}' found in assembly '{assembly.FullName}'.");

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

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (_disposed) return;

			if (!disposing) return;
			_handle.Dispose();
			_disposed = true;
		}
	}
}
