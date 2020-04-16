using System;
using System.Collections.Generic;
using HateoasNet.Abstractions;

namespace HateoasNet.Mapping
{
	public class HateoasLink<T> : IHateoasLink<T> where T : class
	{
		internal HateoasLink(string routeName) : this(routeName, e => null, e => true)
		{
		}

		internal HateoasLink(string routeName,
			Func<T, IDictionary<string, object>> routeDictionaryFunction,
			Func<T, bool> predicateFunction)
		{
			RouteName = routeName;
			RouteDictionaryFunction = routeDictionaryFunction;
			PredicateFunction = predicateFunction;
		}

		public Func<T, bool> PredicateFunction { get; private set; }
		public Func<T, IDictionary<string, object>> RouteDictionaryFunction { get; private set; }
		public string RouteName { get; }

		public IDictionary<string, object> GetRouteDictionary(object routeData)
		{
			if (routeData == null) throw new ArgumentNullException(nameof(routeData));

			return RouteDictionaryFunction(routeData as T);
		}

		public bool IsDisplayable(object routeData)
		{
			if (routeData == null) throw new ArgumentNullException(nameof(routeData));

			return PredicateFunction(routeData as T);
		}

		public IHateoasLink<T> HasRouteData(Func<T, object> routeDataFunction)
		{
			if (routeDataFunction == null) throw new ArgumentNullException(nameof(routeDataFunction));
			
#if NET472
			RouteDictionaryFunction = 
				source => new System.Web.Routing.RouteValueDictionary(routeDataFunction(source));
#else
			RouteDictionaryFunction =
				source => new Microsoft.AspNetCore.Routing.RouteValueDictionary(routeDataFunction(source));
#endif
			return this;
		}

		public IHateoasLink<T> HasConditional(Func<T, bool> predicate)
		{
			PredicateFunction = predicate ?? throw new ArgumentNullException(nameof(predicate));
			return this;
		}
	}
}