using System;
using System.Linq;
using HateoasNet.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
#if NETCOREAPP3_1
using Microsoft.AspNetCore.Mvc.ActionConstraints;

#elif NETSTANDARD2_0
using Microsoft.AspNetCore.Mvc.Internal;
#endif

namespace HateoasNet
{
	internal static class JsonHateoasFormatterExtensions
	{
		private static T GetService<T>(this HttpContext context)
		{
			return (T) context.RequestServices.GetRequiredService(typeof(T));
		}

		internal static T GetPropertyAsValue<T>(this object source, string propertyName)
		{
			return (T) source.GetType().GetProperty(propertyName)?.GetValue(source);
		}

		internal static Resource ApplyHateoasLinks(this Resource resource, Type sourceType, HttpContext context)
		{
			var urlHelperFactory = context.GetService<IUrlHelperFactory>();
			var actionContextAccessor = context.GetService<IActionContextAccessor>();
			var options = context.GetService<IOptions<HateoasNetOptions>>().Value;
			var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
			var actionDescriptors = context
				.GetService<IActionDescriptorCollectionProvider>()
				.ActionDescriptors.Items;

			var displayableLinks = options.Links
				.Where(l => l.SourceType == sourceType && l.CheckLinkPredicate(resource.Data));

			foreach (var link in displayableLinks)
			{
				var route = actionDescriptors.SingleOrDefault(x => x.AttributeRouteInfo.Name == link.RouteName);
				var url = urlHelper.Link(link.RouteName, link.GetRouteDictionary(resource.Data))?.ToLower();

				if (!(route is { }) || !(url is { })) continue;

				var method = route.ActionConstraints.OfType<HttpMethodActionConstraint>().First().HttpMethods.First();
				resource.Links.Add(new ResourceLink(link.RouteName, url, method));
			}

			return resource;
		}
	}
}