using System;
using Microsoft.AspNetCore.Routing;

namespace HateoasNet.Core
{
	public interface IHateoasLink
	{
		Type SourceType { get; }
		string RouteName { get; }
		RouteValueDictionary GetRouteDictionary(object routeData);
		bool CheckLinkPredicate(object routeData);
	}
	
	public interface IHateoasLink<out T> : IHateoasLink where T : class
	{
		IHateoasLink<T> WithRouteData(Func<T, object> routeDataFunction);
		IHateoasLink<T> DisplayedWhen(Func<T, bool> predicate);
	}
}