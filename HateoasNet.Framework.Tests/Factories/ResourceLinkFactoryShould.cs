using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using HateoasNet.Abstractions;
using HateoasNet.Framework.Factories;
using HateoasNet.Resources;
using Moq;
using Xunit;

namespace HateoasNet.Framework.Tests.Factories
{
	public class ResourceLinkFactoryShould
	{
		[Fact]
		[Trait(nameof(IResourceLinkFactory), "Instantiation")]
		public void BoOfType__ResourceLinkFactory()
		{
			var sut = GenerateDefaultSut();

			Assert.IsAssignableFrom<IResourceLinkFactory>(sut);
			Assert.IsType<ResourceLinkFactory>(sut);
		}

		[Theory]
		[UrlResourceLinkData]
		[Trait(nameof(IResourceLinkFactory), nameof(IResourceLinkFactory.Create))]
		public void ReturnsResourceLink_FromCalling_Create(ResourceLinkFactoryTestData data)
		{
			// arrange
			var sut = GenerateFullSut(data);

			// act
			var resourceLink = sut.Create(data.RouteName, data.RouteValues);

			// assert
			Assert.IsType<ResourceLink>(resourceLink);
			Assert.NotNull(resourceLink);
			Assert.Equal(data.ExpectedUrl, resourceLink.Href, StringComparer.OrdinalIgnoreCase);
			Assert.Equal(data.RouteName, resourceLink.Rel, StringComparer.OrdinalIgnoreCase);
			Assert.Equal(data.Method, resourceLink.Method, StringComparer.OrdinalIgnoreCase);
		}

		[Theory, UrlResourceLinkData, Trait(nameof(IResourceLinkFactory), nameof(ResourceLinkFactory.GetRouteUrl))]
		public void ReturnsString_FromCalling_GetRouteUrl(ResourceLinkFactoryTestData data)
		{
			var sut = GenerateFullSut(data);

			// act
			var actual = sut.GetRouteUrl(data.RouteName, data.RouteValues);

			// assert
			Assert.Equal(data.ExpectedUrl, actual, StringComparer.OrdinalIgnoreCase);
		}

		[Theory]
		[InlineData("")]
		[InlineData(null)]
		[Trait(nameof(IResourceLinkFactory), nameof(IResourceLinkFactory.Create))]
		[Trait(nameof(IResourceLinkFactory), "Exceptions")]
		public void Throws_ArgumentNullException_FromCalling_Create_Using_routeName_NullOrWhiteSpace(string rel)
		{
			var sut = GenerateDefaultSut();

			Assert.Throws<ArgumentNullException>("rel", () => sut.Create(rel, null));
		}

		[Theory]
		[InlineData("")]
		[InlineData(null)]
		[Trait(nameof(IResourceLinkFactory), nameof(ResourceLinkFactory.GetRouteUrl))]
		[Trait(nameof(IResourceLinkFactory), "Exceptions")]
		public void Throws_ArgumentNullException_FromCalling_GetRouteUrl(string routeName)
		{
			// arrange
			var sut = GenerateDefaultSut();

			// assert
			Assert.Throws<ArgumentNullException>("routeName", () => sut.GetRouteUrl(routeName, null));
		}

		ResourceLinkFactory GenerateDefaultSut()
		{
			return new ResourceLinkFactory(new[] { new Mock<HttpActionDescriptor>().Object});
		}

		// manually arrange aspnet web api dummies and mocks
		ResourceLinkFactory GenerateFullSut(ResourceLinkFactoryTestData data)
		{
			var config = new Mock<HttpConfiguration>().Object;
			var prefix = new RoutePrefixAttribute(data.Prefix);
			var controllerDescriptor = new TestControllerDescriptor(config, data.ControllerName, typeof(ApiController), prefix);
			var actionParameters =
				data.RouteValues.Keys.Aggregate(new Collection<HttpParameterDescriptor>(),
				                                (collection, parameterName) =>
				                                {
					                                var mockParameterDescriptor = new Mock<HttpParameterDescriptor>();
					                                mockParameterDescriptor.Setup(x => x.ParameterName).Returns(parameterName);
					                                collection.Add(mockParameterDescriptor.Object);
					                                return collection;
				                                });
			var methodInfo = new TestMethodInfo(new RouteAttribute(data.Template) {Name = data.RouteName});
			var actionDescriptor = new TestActionDescriptor(controllerDescriptor, methodInfo, actionParameters);
			actionDescriptor.SupportedHttpMethods.Add(new HttpMethod(data.Method));

			// httpContext
			var request = new HttpRequest("", data.BaseUrl, "");
			var response = new HttpResponse(new StringWriter());
			HttpContext.Current = new HttpContext(request, response);

			return new ResourceLinkFactory(new[] {actionDescriptor});
		}
	}
}
