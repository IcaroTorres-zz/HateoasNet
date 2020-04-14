using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HateoasNet.Abstractions;
using Microsoft.AspNetCore.Routing;

namespace HateoasNet.Core
{
	public class HateoasConfiguration : IHateoasConfiguration
	{
		private readonly Dictionary<Type, IHateoasMap> _maps = new Dictionary<Type, IHateoasMap>();
		private readonly string _genericBuilderName = typeof(IHateoasBuilder<>).Name;

		public IEnumerable<IHateoasLink> GetMappedLinks(Type sourceType, object resourceData)
		{
			return _maps[sourceType].GetLinks().Where(link => link.IsDisplayable(resourceData));
		}

		public IHateoasConfiguration Map<T>(Action<HateoasMap<T>> mapper) where T : class
		{
			if (mapper == null) throw new ArgumentNullException(nameof(mapper));

			mapper(GetOrInsert<T>());

			return this;
		}

		public IHateoasConfiguration ApplyConfiguration<T>(IHateoasBuilder<T> builder) where T : class
		{
			builder.Build(GetOrInsert<T>());

			return this;
		}

		public IHateoasConfiguration ApplyConfigurationsFromAssembly(Assembly assembly)
		{
			var builderTypes = assembly.GetTypes()
				.Where(predicate: t => t.GetInterfaces()
					.Any(predicate: i => i.IsGenericType && i.Name.Contains(_genericBuilderName))).ToList();

			builderTypes.ForEach(builderType =>
			{
				if (!(builderType.GetInterfaces().SingleOrDefault(i => i.Name.Contains(_genericBuilderName)) is {}
					interfaceType)) return;

				var targetType = interfaceType.GetGenericArguments().First();
				var builder = Activator.CreateInstance(builderType);
				var buildMethod = builderType.GetMethod(nameof(IHateoasBuilder<object>.Build));
				var hateoasMap = GetOrInsert(targetType);
				buildMethod.Invoke(builder, new object[] {hateoasMap});
			});

			return this;
		}

		private HateoasMap<T> GetOrInsert<T>() where T : class
		{
			var targetType = typeof(T);

			if (!_maps.ContainsKey(targetType)) _maps.Add(targetType, new HateoasMap<T>());

			return (HateoasMap<T>) _maps[targetType];
		}

		private IHateoasMap GetOrInsert(Type targetType)
		{
			if (_maps.ContainsKey(targetType)) return _maps[targetType];
			var hateoasMapType = typeof(HateoasMap<>).MakeGenericType(targetType);
			_maps.Add(targetType, (IHateoasMap) Activator.CreateInstance(hateoasMapType));

			return _maps[targetType];
		}
	}
}