using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Routing;
using HateoasNet.Abstractions;
using HateoasNet.Resources;

namespace HateoasNet.Framework.Resources
{
	public class ResourceLinkFactory : IResourceLinkFactory
	{
		private string BuildMessageFor(string targetName)
		{
			return $"Unable to get '{targetName}' needed from route.";
		}

		public ResourceLink Create(Resource resource, IHateoasLink hateoasLink)
		{
			if (resource == null) throw new ArgumentNullException(nameof(resource));
			if (hateoasLink == null) throw new ArgumentNullException(nameof(hateoasLink));

			var descriptor = GetActionDescriptor(hateoasLink);
			var method = GetMethod(descriptor).Method;
			var routeAttribute = GetRouteAttribute(descriptor);
			var routeDictionary = hateoasLink.GetRouteDictionary(resource.Data);
			var resourceUrl = BuildResourceUrl(descriptor, routeAttribute.Template, routeDictionary);

			return new ResourceLink(hateoasLink.RouteName, resourceUrl, method);
		}

		private HttpActionDescriptor GetActionDescriptor(IHateoasLink hateoasLink)
		{
			var actionDescriptors = RouteTable.Routes
				.OfType<Route>()
				.Select(route => route.DataTokens.Values.OfType<HttpActionDescriptor[]>().FirstOrDefault())
				.Where(httpActionDescriptors => httpActionDescriptors != null && httpActionDescriptors.Any()).ToArray();

			var descriptorAndAttribute = actionDescriptors
				                             .Select(httpActionDescriptors => new
				                             {
					                             descriptor = httpActionDescriptors.First(),
					                             routeAttribute = GetRouteAttribute(httpActionDescriptors.First())
				                             })
				                             .Single(x => x.routeAttribute.Name == hateoasLink.RouteName)
			                             ?? throw new InvalidOperationException(
				                             BuildMessageFor(nameof(HttpActionDescriptor)));

			return descriptorAndAttribute.descriptor;
		}

		private HttpMethod GetMethod(HttpActionDescriptor descriptor)
		{
			return descriptor.SupportedHttpMethods.FirstOrDefault()
			       ?? throw new InvalidOperationException(BuildMessageFor(nameof(HttpMethod)));
		}

		private RouteAttribute GetRouteAttribute(HttpActionDescriptor descriptor)
		{
			var methodInfo = descriptor.GetType().GetProperty("MethodInfo")?.GetValue(descriptor) as MethodInfo ??
			                 throw new NullReferenceException(BuildMessageFor(nameof(MethodInfo)));

			return methodInfo.GetCustomAttribute<RouteAttribute>() ??
			       throw new NullReferenceException(BuildMessageFor(nameof(RouteAttribute)));
		}

		private string BuildResourceUrl(HttpActionDescriptor descriptor, string template,
			IDictionary<string, object> routeDictionary)
		{
			if (descriptor == null) throw new ArgumentNullException(nameof(descriptor));

			var baseUrl = HttpContext.Current.Request.Url;

			// set resource name from controller
			var controllerDescriptor = descriptor.ControllerDescriptor;
			var routePrefixAttribute = controllerDescriptor.GetCustomAttributes<RoutePrefixAttribute>().SingleOrDefault();
			var resourceName = routePrefixAttribute != null
				? routePrefixAttribute.Prefix
				: controllerDescriptor.ControllerName;

			var resourceUrl = $"{baseUrl.Scheme}://{baseUrl.Host}:{baseUrl.Port}/{resourceName}";
			resourceUrl = HandleRouteTemplate(resourceUrl, template, routeDictionary);

			// parameters for possible query strings
			var parameterDescriptors = descriptor.GetParameters().ToDictionary(x => x.ParameterName);

			return HandleQueryStrings(resourceUrl, parameterDescriptors, template, routeDictionary);
		}

		private string HandleRouteTemplate(string resourceUrl, string template, IDictionary<string, object> routeDictionary)
		{
			if (resourceUrl == null) throw new ArgumentNullException(nameof(resourceUrl));
			if (string.IsNullOrWhiteSpace(template)) return resourceUrl;
			if (routeDictionary == null) return $"{resourceUrl}/{template}";

			var replacedTemplate = template;
			const string fromRouteVariablePattern = @"\{(.*?)\}";
			const string variableIdentifierPattern = @"\w(\w|\d|_)*";

			foreach (Match match in Regex.Matches(replacedTemplate, fromRouteVariablePattern))
			{
				var key = match.Value.Replace("{", "").Replace("}", "");
				if (!routeDictionary.TryGetValue(key, out var replacement))
				{
					key = Regex.Matches(match.Value, variableIdentifierPattern)[0].Value;
					if (!routeDictionary.TryGetValue(key, out replacement))
						throw new InvalidOperationException($"Unable to find key '{key}' from dictionary of route values.");
				}

				replacedTemplate = replacedTemplate.Replace(match.Value, replacement?.ToString());
			}

			return $"{resourceUrl}/{replacedTemplate}";
		}

		private string HandleQueryStrings(string resourceUrl,
			IDictionary<string, HttpParameterDescriptor> parameterDescriptors,
			string routeTemplate, IDictionary<string, object> routeDictionary)
		{
			if (resourceUrl == null) throw new ArgumentNullException(nameof(resourceUrl));
			if (routeDictionary == null) return resourceUrl;

			if (parameterDescriptors == null) throw new ArgumentNullException(nameof(parameterDescriptors));
			if (routeTemplate == null) throw new ArgumentNullException(nameof(routeTemplate));

			return parameterDescriptors
				.Where(p => routeDictionary.ContainsKey(p.Key))
				.Where(p => !routeTemplate.Contains($"{{{p.Key}"))
				.OrderBy(p => p.Key)
				.Aggregate(resourceUrl,
					(query, pair) => $"{query}{(query == resourceUrl ? "?" : "&")}{pair.Key}={routeDictionary[pair.Key]}");
		}
	}
}