﻿using System.Collections.Generic;

namespace HateoasNet.Tests.TestHelpers
{

    public class HateoasTestData
    {
        public string BaseUrl { get; set; }
        public string ControllerName { get; set; }
        public string Prefix { get; set; }
        public string RouteName { get; set; }
        public string Method { get; set; }
        public string Template { get; set; }
        public string ExpectedUrl { get; set; }
        public IDictionary<string, object> RouteValues { get; set; }
    }
}