using System;
using HateoasNet.Abstractions;
using Microsoft.AspNetCore.Routing;

namespace HateoasNet.Mapping
{
	public class HateoasLink<T> : IHateoasLink<T> where T : class
	{
		internal HateoasLink(string routeName,
			Func<T, RouteValueDictionary> routeDictionaryFunction,
			Func<T, bool> predicateFunction)
		{
			RouteName = routeName;
			RouteDictionaryFunction = routeDictionaryFunction;
			PredicateFunction = predicateFunction;
		}

		public Func<T, bool> PredicateFunction { get; private set; }
		public Func<T, RouteValueDictionary> RouteDictionaryFunction { get; private set; }
		public string RouteName { get; }

		public RouteValueDictionary GetRouteDictionary(object routeData)
		{
			return RouteDictionaryFunction(routeData as T);
		}

		public bool IsDisplayable(object routeData)
		{
			return PredicateFunction(routeData as T);
		}

		public IHateoasLink<T> HasRouteData(Func<T, object> routeDataFunction)
		{
			if (routeDataFunction == null) throw new ArgumentNullException(nameof(routeDataFunction));

			RouteDictionaryFunction = source => new RouteValueDictionary(routeDataFunction(source));
			return this;
		}

		public IHateoasLink<T> HasConditional(Func<T, bool> predicate)
		{
			PredicateFunction = predicate ?? throw new ArgumentNullException(nameof(predicate));
			return this;
		}
	}
}