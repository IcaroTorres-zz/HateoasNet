using System.Collections.Generic;
using System.Linq;

namespace HateoasNet.Tests.TestHelpers
{
    public class HateoasTestData
    {
        private readonly object _value;
        private string _baseUrl = "http://hateoasnet.io.sample:5000";

        public HateoasTestData(string routeName, string controller, string method,
            Dictionary<string, object> routeValues = null, string customTemplate = null)
        {
            RouteName = routeName;
            ControllerName = controller;
            Method = method;
            RouteValues = routeValues ?? new Dictionary<string, object>();
            if (RouteValues.Count > 0)
            {
                var pair = RouteValues.First();
                Template = customTemplate ?? $"{{{pair.Key}}}";
                _value = pair.Value;
            }
            else
            {
                Template = "";
                _value = null;
            }
        }

        public string RouteName { get; }
        public string ControllerName { get; }
        public string Method { get; set; }
        public IDictionary<string, object> RouteValues { get; }

        public string BaseUrl { get => _baseUrl.ToLowerInvariant(); set { _baseUrl = value; } }
        public string Prefix => $"api/v1/{ControllerName}";
        public string Template { get; }
        public string RoutePath => Method == "POST" ? Prefix : $"{Prefix}/{_value}";
        public string ExpectedUrl => $"{BaseUrl}/{RoutePath}";
    }
}
