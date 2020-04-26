using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using HateoasNet.Abstractions;
using HateoasNet.Configurations;

namespace HateoasNet.Framework.Resources
{
	public class UrlBuilder : IUrlBuilder
	{
		private readonly IEnumerable<HttpActionDescriptor> _actionDescriptors;

		public UrlBuilder(IEnumerable<HttpActionDescriptor> actionDescriptors)
		{
			_actionDescriptors = actionDescriptors;
		}

		public string Build(string routeName, object routeData)
		{
			if (routeData == null) throw new ArgumentNullException(nameof(routeData));

			return Build(routeName, routeData.ToRouteDictionary());
		}

		public string Build(string routeName, IDictionary<string, object> routeDictionary)
		{
			if (routeName == null) throw new ArgumentNullException(nameof(routeName));

			var (descriptor, routeAttribute) = _actionDescriptors.GetActionDescriptorData(routeName);
			return BuildResourceUrl(descriptor, routeAttribute.Template, routeDictionary ?? new Dictionary<string, object>());
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

			var resourceUrl = $"{baseUrl.Scheme}://{baseUrl.Host}:{baseUrl.Port.ToString()}/{resourceName}";
			resourceUrl = HandleRouteTemplate(resourceUrl, template, routeDictionary);

			// parameters for possible query strings
			var parameters = descriptor.GetParameters().ToDictionary(x => x.ParameterName);

			return HandleQueryStrings(resourceUrl, template, parameters, routeDictionary);
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

		private string HandleQueryStrings(string resourceUrl, string routeTemplate,
		                                  IDictionary<string, HttpParameterDescriptor> parameterDescriptors,
		                                  IDictionary<string, object> routeDictionary)
		{
			if (resourceUrl == null) throw new ArgumentNullException(nameof(resourceUrl));
			if (routeDictionary == null) return resourceUrl;

			if (parameterDescriptors == null) throw new ArgumentNullException(nameof(parameterDescriptors));
			if (routeTemplate == null) throw new ArgumentNullException(nameof(routeTemplate));

			return parameterDescriptors
			       .Where(p => routeDictionary.ContainsKey(p.Key))
			       .Where(p => !routeTemplate.Contains($"{{{p.Key}"))
			       .OrderBy(p => p.Key)
			       .Aggregate(resourceUrl, (query, pair) =>
			       {
				       var symbol = (query == resourceUrl ? "?" : "&");
				       return $"{query}{symbol}{pair.Key}={routeDictionary[pair.Key]}";
			       });
		}
	}
}
