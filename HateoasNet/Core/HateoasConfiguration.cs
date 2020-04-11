using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HateoasNet.Core.Abstractions;
using Microsoft.AspNetCore.Routing;

namespace HateoasNet.Core
{
	public sealed class HateoasConfiguration
	{
		private readonly Dictionary<Type, IHateoasMap> _maps = new Dictionary<Type, IHateoasMap>();
		private readonly List<IHateoasLink> _links = new List<IHateoasLink>();
		private static readonly string GENERIC_BUILDER_NAME = typeof(IHateoasBuilder<>).Name;

		internal HashSet<IHateoasLink> GetConfiguredLinks()
		{
			var linksFromMaps = _maps.Values.SelectMany(map => map.GetLinks());

			return new HashSet<IHateoasLink>(_links.Union(linksFromMaps));
		}

		public IHateoasLink<T> HasLink<T>(string routeName,
			Func<T, object> objectFunction = null,
			Func<T, bool> predicate = null) where T : class
		{
			var valuesFunction = new Func<T, RouteValueDictionary>(
				sourceValue => new RouteValueDictionary(
					(objectFunction ?? (e => null))(sourceValue)
				)
			);
			predicate ??= t => true;

			var hateoasLink = new HateoasLink<T>(routeName, valuesFunction, predicate);
			_links.Add(hateoasLink);
			return hateoasLink;
		}

		public void Map<T>(Action<HateoasMap<T>> mapper) where T : class
		{
			if (mapper == null) throw new ArgumentNullException(nameof(mapper));

			mapper(GetOrInsert<T>());
		}

		public void ApplyConfiguration<T>(IHateoasBuilder<T> builder) where T : class
		{
			builder.Build(GetOrInsert<T>());
		}

		public void ApplyConfigurationsFromAssembly(Assembly assembly)
		{
			var builderTypes = assembly.GetTypes()
				.Where(predicate: t => t.GetInterfaces()
					.Any(predicate: i => i.IsGenericType && i.Name.Contains(GENERIC_BUILDER_NAME))).ToList();

			builderTypes.ForEach(builderType =>
			{
				if (!(builderType.GetInterfaces().SingleOrDefault(i => i.Name.Contains(GENERIC_BUILDER_NAME)) is {}
					interfaceType)) return;

				var targetType = interfaceType.GetGenericArguments().First();
				var builder = Activator.CreateInstance(builderType);
				var buildMethod = builderType.GetMethod(nameof(IHateoasBuilder<object>.Build));
				var hateoasMap = GetOrInsert(targetType);
				buildMethod.Invoke(builder, new object[] {hateoasMap});
			});
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