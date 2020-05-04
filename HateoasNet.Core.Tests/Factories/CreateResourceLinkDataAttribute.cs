using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace HateoasNet.Core.Tests.Factories
{
    public class CreateResourceLinkDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var routeData = new { id = Guid.NewGuid() };
            var routeName = "test1";
            var url = $"http://dummy.test/api/dummy/{routeName}/{routeData.id}";
            var method = "GET";

            yield return new object[] { routeData, routeName, url, method };

            routeName = "test2";
            url = $"http://dummy.test/api/dummy/{routeName}";
            method = "POST";

            yield return new object[] { null, routeName, url, method };

            routeData = new { id = Guid.NewGuid() };
            routeName = "test3";
            url = $"http://dummy.test/api/dummy/{routeName}/{routeData.id}";
            method = "PATCH";

            yield return new object[] { routeData, routeName, url, method };

            routeData = new { id = Guid.NewGuid() };
            routeName = "test4";
            url = $"http://dummy.test/api/dummy/{routeName}/{routeData.id}";
            method = "DELETE";

            yield return new object[] { routeData, routeName, url, method };

            routeName = "test5";
            url = $"http://dummy.test/api/dummy/{routeName}";
            method = "PUT";

            yield return new object[] { routeData, routeName, url, method };

            var routeData2 = new
            {
                id = Guid.NewGuid(),
                label = "test",
                page = 21,
                pageSize = 10
            };
            routeName = "test6";
            var queryString = $"?pageSize={routeData2.pageSize}&page={routeData2.page}&label={routeData2.label}";
            url = $"http://dummy.test/api/dummy/{routeName}{queryString}";
            method = "GET";

            yield return new object[] { routeData2, routeName, url, method };
        }
    }
}
