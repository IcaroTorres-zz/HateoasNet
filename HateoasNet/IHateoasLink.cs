using System;
using Microsoft.AspNetCore.Routing;

namespace HateoasNet
{
	public interface IHateoasLink
	{
		Type SourceType { get; }
		string RouteName { get; }
		RouteValueDictionary GetRouteDictionary(object routeData);
		bool CheckLinkPredicate(object routeData);
	}
}