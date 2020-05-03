using System;
using System.Collections.Generic;
using HateoasNet.Abstractions;
using HateoasNet.Configurations;
using HateoasNet.Core.Factories;
using HateoasNet.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace HateoasNet.Core.Tests.Factories
{
	public class ResourceLinkFactoryShould
	{
		public ResourceLinkFactoryShould()
		{
			_mockActionContextAccessor = new Mock<IActionContextAccessor>().SetupAllProperties();
			_mockUrlHelper = new Mock<IUrlHelper>().SetupAllProperties();
		}

		private readonly Mock<IUrlHelper> _mockUrlHelper;
		private readonly Mock<IActionContextAccessor> _mockActionContextAccessor;

		[Theory]
		[CreateResourceLinkData]
		[Trait(nameof(IResourceLinkFactory), "Create")]
		public void ReturnsResourceLink_FromCalling_Create(object routeData, string routeName, string url, string method)
		{
			// arrange
			MocksActionContextArrangements(routeName, method);
			var routeDictionary = routeData?.ToRouteDictionary();
			_mockUrlHelper.Setup(x => x.Link(routeName, routeDictionary)).Returns(url);

			var sut = new ResourceLinkFactory(_mockUrlHelper.Object, _mockActionContextAccessor.Object);
			// act
			var resourceLink = sut.Create(routeName, routeDictionary);

			// assert
			Assert.IsType<ResourceLink>(resourceLink);
			Assert.NotNull(resourceLink);
			Assert.Equal(url, resourceLink.Href);
			Assert.Equal(routeName, resourceLink.Rel);
			Assert.Equal(method, resourceLink.Method);
		}

		[Theory]
		[InlineData("")]
		[InlineData(null)]
		[Trait(nameof(IResourceLinkFactory), "Create")]
		[Trait(nameof(IResourceLinkFactory), "Exceptions")]
		public void Throws_ArgumentNullException_FromCalling_Create_Using_routeName_NullOrWhiteSpace(string rel)
		{
			// arrange action context dummy data
			MocksActionContextArrangements(rel);

			// mocking methods from dependencies
			var sut = new ResourceLinkFactory(_mockUrlHelper.Object, _mockActionContextAccessor.Object);

			// assert
			Assert.Throws<ArgumentNullException>("rel", () => sut.Create(rel, null));
		}

		void MocksActionContextArrangements(string routeName, string method = "GET")
		{
			var httpContext = new Mock<HttpContext>().SetupAllProperties().Object;
			var actionRouteData = new Mock<RouteData>().SetupAllProperties().Object;
			var actionDescriptor = new ActionDescriptor
			{
				AttributeRouteInfo = new AttributeRouteInfo {Name = routeName},
				ActionConstraints = new List<IActionConstraintMetadata>
					{new HttpMethodActionConstraint(new List<string>() {method})}
			};
			var actionContext = new ActionContext(httpContext, actionRouteData, actionDescriptor);

			_mockActionContextAccessor.Setup(x => x.ActionContext).Returns(actionContext);
		}
	}
}
