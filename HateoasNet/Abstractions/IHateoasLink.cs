using System;
using System.Collections.Generic;

namespace HateoasNet.Abstractions
{
	public interface IHateoasLink
	{
		string RouteName { get; }
		IDictionary<string, object> GetRouteDictionary(object routeData);
		bool IsDisplayable(object routeData);
	}

	public interface IHateoasLink<T> : IHateoasLink where T : class
	{
		Func<T, bool> PredicateFunction { get; }
		Func<T, IDictionary<string, object>> RouteDictionaryFunction { get; }
		
		IHateoasLink<T> HasRouteData(Func<T, object> routeDataFunction);
		IHateoasLink<T> HasConditional(Func<T, bool> predicate);
	}
}