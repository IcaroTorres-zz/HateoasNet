using System;
using Microsoft.AspNetCore.Routing;

namespace HateoasNet.Core.Abstractions
{
	public interface IHateoasLink
	{
		Type SourceType { get; }
		string RouteName { get; }
		RouteValueDictionary GetRouteDictionary(object routeData);
		bool CheckPredicateForData(object routeData);
	}
	
	public interface IHateoasLink<out T> : IHateoasLink where T : class
	{
		IHateoasLink<T> WithData(Func<T, object> routeDataFunction);
		IHateoasLink<T> When(Func<T, bool> predicate);
	}
}