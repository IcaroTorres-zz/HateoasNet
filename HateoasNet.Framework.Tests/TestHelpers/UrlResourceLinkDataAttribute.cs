using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace HateoasNet.Framework.Tests.TestHelpers
{
	public class UrlResourceLinkDataAttribute : DataAttribute
	{
		/// <inheritdoc />
		public override IEnumerable<object[]> GetData(MethodInfo testMethod)
		{
			var id = Guid.NewGuid();
			var baseUrls = new[]
			{
				"http://hateoasNet.io:5000/api",
				"https://hateoasNet.framework:5001/api",
				"http://hateoasNet.core:5002/api",
				"http://hateoasNet.dummy:5000/api/v1",
				"http://hateoasNet.test:5000/api/v2",
			};
			var controllerNames = Enumerable.Range(1, 5).Select(i => "controller" + i).ToArray();
			var testRoutes = Enumerable.Range(1, 5).Select(i => "route" + i).ToArray();

			yield return new object[]
			{
				new ResourceLinkFactoryTestData
				{
					BaseUrl = baseUrls[0],
					ControllerName = controllerNames[0],
					Prefix = $"api/{controllerNames[0]}",
					RouteName = testRoutes[0],
					Method = "DELETE",
					Template = "{id:guid}",
					RouteValues = new Dictionary<string, object> {{"id", id}},
					ExpectedUrl = $"{baseUrls[0]}/{controllerNames[0]}/{id}",
				}
			};

			yield return new object[]
			{
				new ResourceLinkFactoryTestData
				{
					BaseUrl = baseUrls[1],
					ControllerName = controllerNames[1],
					Prefix = $"api/{controllerNames[1]}",
					RouteName = testRoutes[1],
					Method = "PATCH",
					Template = "{id:guid}",
					RouteValues = new Dictionary<string, object> {{"id", id}},
					ExpectedUrl = $"{baseUrls[1]}/{controllerNames[1]}/{id}",
				}
			};

			yield return new object[]
			{
				new ResourceLinkFactoryTestData
				{
					BaseUrl = baseUrls[2],
					ControllerName = controllerNames[2],
					Prefix = $"api/{controllerNames[2]}",
					RouteName = testRoutes[2],
					Method = "PUT",
					Template = "{id}",
					RouteValues = new Dictionary<string, object> {{"id", id}},
					ExpectedUrl = $"{baseUrls[2]}/{controllerNames[2]}/{id}",
				}
			};

			yield return new object[]
			{
				new ResourceLinkFactoryTestData
				{
					BaseUrl = baseUrls[3],
					ControllerName = controllerNames[3],
					Prefix = $"api/v1/{controllerNames[3]}",
					RouteName = testRoutes[3],
					Method = "POST",
					Template = "",
					RouteValues = new Dictionary<string, object>(),
					ExpectedUrl = $"{baseUrls[3]}/{controllerNames[3]}",
				}
			};

			yield return new object[]
			{
				new ResourceLinkFactoryTestData
				{
					BaseUrl = baseUrls[4],
					ControllerName = controllerNames[4],
					Prefix = $"api/v2/{controllerNames[4]}",
					RouteName = testRoutes[4],
					Method = "GET",
					Template = "",
					RouteValues = new Dictionary<string, object> {{"relationId", id}},
					ExpectedUrl = $"{baseUrls[4]}/{controllerNames[4]}?relationId={id}",
				},
			};
		}
	}
}
