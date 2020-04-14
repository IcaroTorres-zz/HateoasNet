using System;
using Microsoft.AspNetCore.Routing;

namespace HateoasNet.Abstractions
{
	public interface IHateoasLink
	{
		string RouteName { get; }
		RouteValueDictionary GetRouteDictionary(object routeData);
		bool IsDisplayable(object routeData);
	}

	public interface IHateoasLink<out T> : IHateoasLink where T : class
	{
		IHateoasLink<T> HasRouteData(Func<T, object> routeDataFunction);
		IHateoasLink<T> HasConditional(Func<T, bool> predicate);
	}
}