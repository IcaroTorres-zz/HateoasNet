using System;
using System.Collections.Generic;
using HateoasNet.Abstractions;
using HateoasNet.Configurations;
using HateoasNet.Core.Factories;
using HateoasNet.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using Xunit;

namespace HateoasNet.Core.Tests.Factories
{
	public class ResourceLinkFactoryTests : IDisposable
	{
		public ResourceLinkFactoryTests()
		{
			_mockActionDescriptorCollectionProvider = new Mock<IActionDescriptorCollectionProvider>().SetupAllProperties();
			_mockUrlHelper = new Mock<IUrlHelper>().SetupAllProperties();
		}

		/// <inheritdoc />
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		private readonly Mock<IUrlHelper> _mockUrlHelper;
		private readonly Mock<IActionDescriptorCollectionProvider> _mockActionDescriptorCollectionProvider;

		[Theory]
		[CreateResourceLinkData]
		[Trait(nameof(IResourceLinkFactory), nameof(IResourceLinkFactory.Create))]
		public void Create_ValidParameters_ReturnsResourceLink(object data, string routeName, string url, string method)
		{
			// arrange
			ActionDescriptorCollectionProviderArrangements(routeName, method);
			var routeDictionary = data?.ToRouteDictionary(); // can be null
			_mockUrlHelper.Setup(x => x.Link(routeName, routeDictionary)).Returns(url);
			var sut = new ResourceLinkFactory(_mockUrlHelper.Object, _mockActionDescriptorCollectionProvider.Object);

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
		[Trait(nameof(IResourceLinkFactory), nameof(IResourceLinkFactory.Create))]
		[Trait(nameof(IResourceLinkFactory), "Exceptions")]
		public void Create_WithRouteNameNull_Throws_ArgumentNullException(string rel)
		{
			// arrange
			ActionDescriptorCollectionProviderArrangements(rel);
			var sut = new ResourceLinkFactory(_mockUrlHelper.Object, _mockActionDescriptorCollectionProvider.Object);
			const string parameterName = "rel";

			// act
			Action actual = () => sut.Create(rel, It.IsAny<IDictionary<string, object>>());

			// assert
			Assert.Throws<ArgumentNullException>(parameterName, actual);
		}

		private void ActionDescriptorCollectionProviderArrangements(string routeName, string method = "GET")
		{
			var actionDescriptors = new List<ActionDescriptor>
			{
				new ActionDescriptor
				{
					AttributeRouteInfo = new AttributeRouteInfo {Name = routeName},
					ActionConstraints = new List<IActionConstraintMetadata>
						{new HttpMethodActionConstraint(new List<string> {method})}
				}
			};
			_mockActionDescriptorCollectionProvider.Setup(x => x.ActionDescriptors)
			                                       .Returns(new ActionDescriptorCollection(actionDescriptors, 1));
		}

		[Fact]
		[Trait(nameof(IResourceLinkFactory), nameof(IResourceLinkFactory.Create))]
		[Trait(nameof(IResourceLinkFactory), "Exceptions")]
		public void Create_WithRouteNotFound_Throws_NotSupportedException()
		{
			// arrange
			const string existentRouteName = "exists";
			const string notExistentRouteName = "notExists";
			ActionDescriptorCollectionProviderArrangements(existentRouteName);
			var sut = new ResourceLinkFactory(_mockUrlHelper.Object, _mockActionDescriptorCollectionProvider.Object);

			// act
			Action actual = () => sut.Create(notExistentRouteName, It.IsAny<IDictionary<string, object>>());

			// assert
			Assert.Throws<NotSupportedException>(actual);
		}
	}
}
