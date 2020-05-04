using HateoasNet.Abstractions;
using HateoasNet.Resources;
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

namespace HateoasNet.Framework.Factories
{
    /// <inheritdoc cref="IResourceLinkFactory" />
    public sealed class ResourceLinkFactory : IResourceLinkFactory
    {
        private Dictionary<RouteAttribute, HttpActionDescriptor> _routeActionDescriptors;
        private readonly RouteAttribute _dummyRouteAttributeInCaseNotFound = new RouteAttribute("unable-to-find-route");

        /// <summary>
        /// 	Constructor for testing purposes.
        /// </summary>
        /// <param name="actionDescriptors"></param>
        public ResourceLinkFactory(IEnumerable<HttpActionDescriptor> actionDescriptors)
        {
            _routeActionDescriptors = actionDescriptors.ToDictionary(GetRouteAttribute);
        }

        public ResourceLinkFactory()
        {
        }

        internal Dictionary<RouteAttribute, HttpActionDescriptor> BuildRouteActionDescriptors()
        {
            var actionDescriptors = RouteTable
                                    .Routes
                                    .OfType<Route>()
                                    .Select(route => route.DataTokens.Values.OfType<HttpActionDescriptor[]>()
                                                          .FirstOrDefault()?.First()).Where(x => x != null);

            return actionDescriptors.ToDictionary(GetRouteAttribute);
        }

        public ResourceLink Create(string rel, IDictionary<string, object> routeValues)
        {
            if (string.IsNullOrWhiteSpace(rel)) throw new ArgumentNullException(nameof(rel));

            _routeActionDescriptors ??= BuildRouteActionDescriptors();

            var href = GetRouteUrl(rel, routeValues);
            var method = GetRouteMethod(rel);
            return new ResourceLink(rel, href, method);
        }

        /// <summary>
        ///   Builds an url <see langword="string" /> with the <paramref name="routeName" /> and <paramref name="routeValues" />.
        /// </summary>
        /// <param name="routeName">Name of desired route to discover the url.</param>
        /// <param name="routeValues">Route dictionary to look for parameters and query strings.</param>
        /// <returns>Generated Url <see langword="string" /> value.</returns>
        internal string GetRouteUrl(string routeName, IDictionary<string, object> routeValues)
        {
            if (string.IsNullOrWhiteSpace(routeName)) throw new ArgumentNullException(nameof(routeName));
            if (HttpContext.Current.Request == null) throw new NullReferenceException(nameof(HttpContext.Current.Request));

            var (routeAttribute, descriptor) = _routeActionDescriptors.Where(pair => pair.Key.Name == routeName)
                                                                      .Select(x => (x.Key, x.Value)).First();

            if (descriptor == null) throw new ArgumentNullException(nameof(descriptor));

            // set resource name from controller
            var controllerDescriptor = descriptor.ControllerDescriptor;
            var routePrefixAttribute = controllerDescriptor.GetCustomAttributes<RoutePrefixAttribute>().FirstOrDefault();
            var resourceName = routePrefixAttribute != null
              ? routePrefixAttribute.Prefix
              : controllerDescriptor.ControllerName;

            var request = HttpContext.Current.Request;
            var segments = $"{request.Url.Authority}{request.ApplicationPath}/{resourceName}".Replace("//", "/");
            var resourceUrl = $"{request.Url.Scheme}://{segments}";

            resourceUrl = HandleRouteTemplate(resourceUrl, routeAttribute.Template, routeValues);

            var parameters = descriptor.GetParameters().Select(x => x.ParameterName);
            return HandleQueryStrings(resourceUrl, routeAttribute.Template, parameters, routeValues);
        }

        /// <summary>
        ///   Find the <see langword="string" /> value of HTTP method of a route with given <paramref name="routeName" />.
        /// </summary>
        /// <param name="routeName">The wanted endpoint route name to find.</param>
        /// <returns><see langword="string" />value representing HTTP method.</returns>
        internal string GetRouteMethod(string routeName)
        {
            if (string.IsNullOrWhiteSpace(routeName)) throw new ArgumentNullException(nameof(routeName));

            var descriptor = _routeActionDescriptors.Where(pair => pair.Key.Name == routeName)
                                                    .Select(pair => pair.Value)
                                                    .Single();

            var httpMethod = descriptor.SupportedHttpMethods.FirstOrDefault() ??
                             throw new InvalidOperationException(Error<HttpMethod>());

            return httpMethod.Method;
        }

        internal string HandleRouteTemplate(string resourceUrl, string template, IDictionary<string, object> routeValues)
        {
            if (string.IsNullOrWhiteSpace(resourceUrl)) throw new ArgumentNullException(nameof(resourceUrl));
            if (string.IsNullOrWhiteSpace(template)) return resourceUrl;
            if (routeValues == null) return $"{resourceUrl}/{template}";

            var replacedTemplate = template;
            const string fromRouteVariablePattern = @"\{(.*?)\}";
            const string variableIdentifierPattern = @"\w(\w|\d|_)*";

            foreach (Match match in Regex.Matches(replacedTemplate, fromRouteVariablePattern))
            {
                var key = match.Value.Replace("{", "").Replace("}", "");
                if (!routeValues.TryGetValue(key, out var replacement))
                {
                    key = Regex.Matches(match.Value, variableIdentifierPattern)[0].Value;
                    if (!routeValues.TryGetValue(key, out replacement))
                        throw new InvalidOperationException($"Unable to find key '{key}' from dictionary of route values.");
                }

                replacedTemplate = replacedTemplate.Replace(match.Value, replacement?.ToString());
            }

            return $"{resourceUrl}/{replacedTemplate}";
        }

        internal string HandleQueryStrings(string resourceUrl, string template, IEnumerable<string> parameterNames,
                                           IDictionary<string, object> routeValues)
        {
            if (resourceUrl == null) throw new ArgumentNullException(nameof(resourceUrl));
            if (routeValues == null) return resourceUrl;

            if (parameterNames == null) throw new ArgumentNullException(nameof(parameterNames));
            if (template == null) throw new ArgumentNullException(nameof(template));

            return parameterNames
                   .Where(routeValues.ContainsKey)
                   .Where(name => !template.Contains($"{{{name}"))
                   .OrderBy(p => p)
                   .Aggregate(resourceUrl, (query, parameterName) =>
                   {
                       var symbol = (query == resourceUrl ? "?" : "&");
                       return $"{query}{symbol}{parameterName}={routeValues[parameterName]}";
                   });
        }

        internal RouteAttribute GetRouteAttribute(HttpActionDescriptor descriptor)
        {
            var methodInfo = descriptor.GetType().GetProperty("MethodInfo")?.GetValue(descriptor) as MethodInfo;

            return methodInfo?.GetCustomAttributes(true).OfType<RouteAttribute>().FirstOrDefault()
                   ?? _dummyRouteAttributeInCaseNotFound;
        }

        private string Error<T>()
        {
            return $"Unable to get '{typeof(T).Name}' needed to create the link.";
        }
    }
}
