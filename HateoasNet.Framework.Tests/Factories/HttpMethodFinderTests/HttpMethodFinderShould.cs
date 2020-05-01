// using System;
// using System.Collections.Generic;
// using HateoasNet.Abstractions;
// using HateoasNet.Core.Resources;
// using Microsoft.AspNetCore.Mvc.Abstractions;
// using Microsoft.AspNetCore.Mvc.ActionConstraints;
// using Microsoft.AspNetCore.Mvc.Infrastructure;
// using Microsoft.AspNetCore.Mvc.Routing;
// using Moq;
// using Xunit;
//
// namespace HateoasNet.Core.Tests.Resources.HttpMethodFinderTests
// {
// 	public class HttpMethodFinderShould
// 	{
// 		[Fact]
// 		[Trait(nameof(IHttpMethodFinder), nameof(IHttpMethodFinder.GetRouteMethod))]
// 		public void ReturnsString_FromCalling_Find()
// 		{
// 			// arrange
// 			const string routeName = "test";
// 			var expected = GetDummyMethod();
// 			var actionDescriptor = new ActionDescriptor
// 			{
// 				AttributeRouteInfo = new AttributeRouteInfo {Name = routeName},
// 				ActionConstraints = new List<IActionConstraintMetadata>
// 					{new HttpMethodActionConstraint(new List<string>() {expected})}
// 			};
//
// 			var mockActionDescriptorCollectionProvider = new Mock<IActionDescriptorCollectionProvider>();
// 			mockActionDescriptorCollectionProvider
// 				.SetupGet(x => x.ActionDescriptors)
// 				.Returns(new ActionDescriptorCollection(new List<ActionDescriptor> {actionDescriptor}, 1));
//
// 			var sut = new HttpMethodFinder(mockActionDescriptorCollectionProvider.Object);
//
// 			// act
// 			var actual = sut.GetRouteMethod(routeName);
//
// 			// assert
// 			Assert.Equal(expected, actual);
// 		}
//
// 		[Theory]
// 		[InlineData("")]
// 		[InlineData(null)]
// 		[Trait(nameof(IHttpMethodFinder), nameof(IHttpMethodFinder.GetRouteMethod))]
// 		[Trait(nameof(IHttpMethodFinder), "Exceptions")]
// 		public void Throws_ArgumentNullException_FromCalling_Find(string routeName)
// 		{
// 			// arrange
// 			var mockActionDescriptorCollectionProvider = new Mock<IActionDescriptorCollectionProvider>();
// 			mockActionDescriptorCollectionProvider
// 				.SetupGet(x => x.ActionDescriptors)
// 				.Returns(new ActionDescriptorCollection(new List<ActionDescriptor>( ), 1));
//
// 			var sut = new HttpMethodFinder(mockActionDescriptorCollectionProvider.Object);
//
// 			// assert
// 			Assert.Throws<ArgumentNullException>("routeName", () => sut.GetRouteMethod(routeName));
// 		}
//
// 		[Fact]
// 		[Trait(nameof(IHttpMethodFinder), nameof(IHttpMethodFinder.GetRouteMethod))]
// 		[Trait(nameof(IHttpMethodFinder), "Exceptions")]
// 		public void Throws_InvalidOperationException_FromCalling_Find()
// 		{
// 			// arrange
// 			var mockActionDescriptorCollectionProvider = new Mock<IActionDescriptorCollectionProvider>();
// 			mockActionDescriptorCollectionProvider
// 				.SetupGet(x => x.ActionDescriptors)
// 				.Returns(new ActionDescriptorCollection(new List<ActionDescriptor>( ), 1));
//
// 			var sut = new HttpMethodFinder(mockActionDescriptorCollectionProvider.Object);
//
// 			// assert
// 			Assert.Throws<InvalidOperationException>(() => sut.GetRouteMethod("value-not-present"));
// 		}
//
// 		string GetDummyMethod() => new[] {"GET", "POST", "PUT", "PATCH", "DELETE"}[new Random().Next(0, 4)];
// 	}
// }
