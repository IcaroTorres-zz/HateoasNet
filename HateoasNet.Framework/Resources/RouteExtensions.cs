using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace HateoasNet.Framework.Resources
{
	public static class RouteExtensions
	{
		internal static (HttpActionDescriptor, RouteAttribute) GetActionDescriptorData(
			this IEnumerable<HttpActionDescriptor> actionDescriptors, string routeName)
		{
			return actionDescriptors.Select(descriptor => (descriptor, descriptor.GetRouteAttribute()))
			                        .SingleOrDefault(x => x.Item2.Name == routeName);
		}

		internal static RouteAttribute GetRouteAttribute(this HttpActionDescriptor descriptor)
		{
			var methodInfo = descriptor.GetType().GetProperty("MethodInfo")?.GetValue(descriptor) as MethodInfo ??
			                 throw new NullReferenceException(Error<MethodInfo>());

			return methodInfo.GetCustomAttribute<RouteAttribute>() ??
			       throw new NullReferenceException(Error<RouteAttribute>());
		}

		internal static HttpMethod GetMethod(this HttpActionDescriptor descriptor)
		{
			return descriptor.SupportedHttpMethods.FirstOrDefault() ??
			       throw new InvalidOperationException(Error<HttpMethod>());
		}

		private static string Error<T>()
		{
			return $"Unable to get '{typeof(T).Name}' needed to create the link.";
		}
	}
}