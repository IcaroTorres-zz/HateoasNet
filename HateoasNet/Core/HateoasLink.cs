using System;
using HateoasNet.Core.Abstractions;
using Microsoft.AspNetCore.Routing;

namespace HateoasNet.Core
{
	public sealed class HateoasLink<T> : IHateoasLink<T> where T : class
	{
		private Func<T, bool> PredicateFunction;
		private Func<T, RouteValueDictionary> ValuesFunction;

		internal HateoasLink(string routeName, Func<T, RouteValueDictionary> valuesFunction, Func<T, bool> predicate)
		{
			RouteName = routeName;
			ValuesFunction = valuesFunction;
			PredicateFunction = predicate;
			SourceType = typeof(T);
		}

		public Type SourceType { get; }
		public string RouteName { get; }

		public RouteValueDictionary GetRouteDictionary(object routeData) => ValuesFunction(routeData as T);

		public bool CheckPredicateForData(object routeData) => PredicateFunction(routeData as T);

		public IHateoasLink<T> WithData(Func<T, object> routeDataFunction)
		{
			if (routeDataFunction == null) throw new ArgumentNullException(nameof(routeDataFunction));

			ValuesFunction = (T source) => new RouteValueDictionary(routeDataFunction(source));
			return this;
		}

		public IHateoasLink<T> When(Func<T, bool> predicate)
		{
			PredicateFunction = predicate ?? throw new ArgumentNullException(nameof(predicate));
			return this;
		}
	}
}