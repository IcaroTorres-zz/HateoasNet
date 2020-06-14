using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace HateoasNet.Tests.TestHelpers
{

    public class HateoasFrameworkDataAttribute : DataAttribute
    {
        /// <inheritdoc />
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var data = new HateoasSample();
            var baseUrls = new[]
            {
                "http://hateoasNet.io:5000/api",
                "https://hateoasNet.framework:5001/api",
                "http://hateoasNet.core:5002/api",
                "http://hateoasNet.dummy:5000/api/v1",
                "http://hateoasNet.test:5000/api/v2"
            };
            var controllerNames = Enumerable.Range(1, 5).Select(i => "Controller" + i).ToArray();
            var testRoutes = Enumerable.Range(1, 5).Select(i => "Route" + i).ToArray();

            yield return new object[]
            {
                new HateoasTestData
                {
                    BaseUrl = baseUrls[0],
                    ControllerName = controllerNames[0],
                    Prefix = $"api/{controllerNames[0]}",
                    RouteName = testRoutes[0],
                    Method = "DELETE",
                    Template = "{id:guid}",
                    RouteValues = new Dictionary<string, object> {{"id", data.Id } },
                    ExpectedUrl = $"{baseUrls[0].ToLowerInvariant()}/{controllerNames[0]}/{data.Id}"
                },
                data
            };

            yield return new object[]
            {
                new HateoasTestData
                {
                    BaseUrl = baseUrls[1],
                    ControllerName = controllerNames[1],
                    Prefix = $"api/{controllerNames[1]}",
                    RouteName = testRoutes[1],
                    Method = "PATCH",
                    Template = "{processNumber:string}",
                    RouteValues = new Dictionary<string, object> {{"processNumber", data.ProcessNumber}},
                    ExpectedUrl = $"{baseUrls[1].ToLowerInvariant()}/{controllerNames[1]}/{data.ProcessNumber}"
                },
                data
            };

            yield return new object[]
            {
                new HateoasTestData
                {
                    BaseUrl = baseUrls[2],
                    ControllerName = controllerNames[2],
                    Prefix = $"api/{controllerNames[2]}",
                    RouteName = testRoutes[2],
                    Method = "PUT",
                    Template = "{zipCode}",
                    RouteValues = new Dictionary<string, object> {{"zipCode", data.ZipCode}},
                    ExpectedUrl = $"{baseUrls[2].ToLowerInvariant()}/{controllerNames[2]}/{data.ZipCode}"
                },
                data
            };

            yield return new object[]
            {
                new HateoasTestData
                {
                    BaseUrl = baseUrls[3],
                    ControllerName = controllerNames[3],
                    Prefix = $"api/v1/{controllerNames[3]}",
                    RouteName = testRoutes[3],
                    Method = "POST",
                    Template = "",
                    RouteValues = new Dictionary<string, object>(),
                    ExpectedUrl = $"{baseUrls[3].ToLowerInvariant()}/{controllerNames[3]}"
                },
                data
            };

            yield return new object[]
            {
                new HateoasTestData
                {
                    BaseUrl = baseUrls[4],
                    ControllerName = controllerNames[4],
                    Prefix = $"api/v2/{controllerNames[4]}",
                    RouteName = testRoutes[4],
                    Method = "GET",
                    Template = "",
                    RouteValues = new Dictionary<string, object> {{"relationId", data.ForeignKeyId}},
                    ExpectedUrl = $"{baseUrls[4].ToLowerInvariant()}/{controllerNames[4]}?relationId={data.ForeignKeyId}"
                },
                data
            };
        }
    }
}
