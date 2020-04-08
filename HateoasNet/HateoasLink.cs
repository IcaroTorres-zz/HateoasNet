using System;
using Microsoft.AspNetCore.Routing;

namespace HateoasNet
{
	internal sealed class HateoasLink<T> : IHateoasLink where T : class
	{
		private readonly Func<T, bool> PredicateFunction;
		private readonly Func<T, RouteValueDictionary> ValuesFunction;

		internal HateoasLink(string routeName, Func<T, RouteValueDictionary> valuesFunction, Func<T, bool> predicate = null)
		{
			RouteName = routeName;
			ValuesFunction = valuesFunction;
			PredicateFunction = predicate ?? (t => true);
			SourceType = typeof(T);
		}

		public Type SourceType { get; }
		public string RouteName { get; }

		public RouteValueDictionary GetRouteDictionary(object routeData)
		{
			return ValuesFunction(routeData as T);
		}

		public bool CheckLinkPredicate(object routeData)
		{
			return PredicateFunction(routeData as T);
		}
	}
}