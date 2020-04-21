using System;
using System.Collections.Generic;
using HateoasNet.Abstractions;

namespace HateoasNet.Mapping
{
	/// <inheritdoc cref="IHateoasLink"/>
	public sealed class HateoasLink<T> : IHateoasLink<T> where T : class
	{
		internal HateoasLink(string routeName) : this(routeName, e => null, e => true)
		{
		}

		private HateoasLink(string routeName,
			Func<T, IDictionary<string, object>> routeDictionaryFunction,
			Func<T, bool> predicateFunction)
		{
			RouteName = routeName;
			RouteDictionaryFunction = routeDictionaryFunction;
			PredicateFunction = predicateFunction;
		}

		public string RouteName { get; }
		public Func<T, bool> PredicateFunction { get; private set; }
		public Func<T, IDictionary<string, object>> RouteDictionaryFunction { get; private set; }

		public IDictionary<string, object> GetRouteDictionary(object resourceData)
		{
			if (resourceData == null) throw new ArgumentNullException(nameof(resourceData));

			return RouteDictionaryFunction(resourceData as T);
		}

		public bool IsApplicable(object resourceData)
		{
			if (resourceData == null) throw new ArgumentNullException(nameof(resourceData));

			return PredicateFunction(resourceData as T);
		}

		public IHateoasLink<T> HasRouteData(Func<T, object> routeDataFunction)
		{
			if (routeDataFunction == null) throw new ArgumentNullException(nameof(routeDataFunction));
			RouteDictionaryFunction = source => routeDataFunction(source).ToRouteDictionary();
			return this;
		}

		public IHateoasLink<T> HasConditional(Func<T, bool> predicate)
		{
			PredicateFunction = predicate ?? throw new ArgumentNullException(nameof(predicate));
			return this;
		}
	}
}