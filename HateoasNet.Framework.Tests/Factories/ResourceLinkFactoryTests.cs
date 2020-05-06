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
	public class ResourceLinkFactoryTests : IDisposable
	{
		/// <inheritdoc />
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		[Theory]
		[UrlResourceLinkData]
		[Trait(nameof(IResourceLinkFactory), nameof(IResourceLinkFactory.Create))]
		public void Create_WithValidParameters_ReturnsValidResourceLink(ResourceLinkFactoryTestData data)
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

		[Theory]
		[UrlResourceLinkData]
		[Trait(nameof(IResourceLinkFactory), nameof(ResourceLinkFactory.GetRouteUrl))]
		public void GetRouteUrl_WithValidParameters_ReturnsRouteUrlString(ResourceLinkFactoryTestData data)
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
		public void Create_WithRouteNameNullOrWhiteSpace_Throws_ArgumentNullException(string rel)
		{
			// arrange
			var sut = GenerateDefaultSut();
			const string parameterName = "rel";

			// act
			Action actual = () => sut.Create(rel, null);

			// act
			Assert.Throws<ArgumentNullException>(parameterName, actual);
		}

		[Theory]
		[InlineData("")]
		[InlineData(null)]
		[Trait(nameof(IResourceLinkFactory), nameof(ResourceLinkFactory.GetRouteUrl))]
		[Trait(nameof(IResourceLinkFactory), "Exceptions")]
		public void GetRouteUrl_WithRouteNameNullOrWhiteSpace_Throws_ArgumentNullException(string routeName)
		{
			// arrange
			var sut = GenerateDefaultSut();
			const string parameterName = "routeName";

			// act
			Action actual = () => sut.GetRouteUrl(routeName, null);

			// assert
			Assert.Throws<ArgumentNullException>(parameterName, actual);
		}

		private ResourceLinkFactory GenerateDefaultSut()
		{
			return new ResourceLinkFactory(new[] {new Mock<HttpActionDescriptor>().Object});
		}

		// manually arrange aspnet web api dummies and mocks
		private ResourceLinkFactory GenerateFullSut(ResourceLinkFactoryTestData data)
		{
			var config = new Mock<HttpConfiguration>().Object;
			var prefix = new RoutePrefixAttribute(data.Prefix);
			var controllerDescriptor =
				new TestControllerDescriptor(config, data.ControllerName, typeof(ApiController), prefix);
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

		[Fact]
		[Trait(nameof(IResourceLinkFactory), "Instantiation")]
		public void New_WithValidParameters_ReturnsResourceLinkFactory()
		{
			// act
			var sut = GenerateDefaultSut();

			// assert
			Assert.IsAssignableFrom<IResourceLinkFactory>(sut);
			Assert.IsType<ResourceLinkFactory>(sut);
		}
	}
}
