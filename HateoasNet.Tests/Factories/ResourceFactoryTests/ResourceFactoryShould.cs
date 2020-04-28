using HateoasNet.Abstractions;
using HateoasNet.Factories;
using HateoasNet.Resources;
using HateoasNet.Tests.TestHelpers;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HateoasNet.Tests.Factories.ResourceFactoryTests
{
	public class ResourceFactoryShould
	{
		public ResourceFactoryShould()
		{
			_mockHateoasContext = new Mock<IHateoasContext>();
			_mockResourceLinkFactory = new Mock<IResourceLinkFactory>();
			_sut = new ResourceFactory(_mockHateoasContext.Object, _mockResourceLinkFactory.Object);
		}

		private readonly Mock<IHateoasContext> _mockHateoasContext;
		private readonly Mock<IResourceLinkFactory> _mockResourceLinkFactory;
		private readonly IResourceFactory _sut;

		[Theory]
		[CreateResourceDataAttribute]
		[Trait(nameof(IResourceFactory), nameof(IResourceFactory.Create))]
		public void ReturnsResource_FromCalling_Create<T>(T source) where T : class
		{
			// arrange
			var resourceLink = GetResourceLinkFromMockArrangements<T>();

			var innerLinks = new List<ResourceLink>();

			Resource resource;
			// act
			switch (source)
			{
				case IEnumerable enumerable:
					var enumerableResource = _sut.Create(enumerable, typeof(T));
					innerLinks = enumerableResource.EnumerableData.SelectMany(x => x.Links).ToList();
					resource = enumerableResource;
					break;
				case IPagination pagination:
					var paginationResource = _sut.Create(pagination, typeof(T));
					innerLinks = paginationResource.EnumerableData.SelectMany(x => x.Links).ToList();
					resource = paginationResource;
					break;
				default:
					resource = _sut.Create(source, typeof(T));
					break;
			}

			// assert
			Assert.NotNull(resource);
			Assert.IsAssignableFrom<Resource>(resource);
			Assert.IsType<List<ResourceLink>>(resource.Links);
			Assert.Contains(resourceLink, resource.Links);
			Assert.True(innerLinks.All(l => l == resourceLink));
		}

		[Theory]
		[BuildResourceLinksData]
		[Trait(nameof(IResourceFactory), nameof(IResourceFactory.BuildResourceLinks))]
		public void AddsLinksToResource_FromCalling_BuildResourceLinks<T>(Resource resource, T source, Type type)
			where T : class

		{
			// arrange
			var wasEmptyBeforeAct = !resource.Links.Any();
			var resourceLink = GetResourceLinkFromMockArrangements<T>();

			// act
			_sut.BuildResourceLinks(resource, source, type);

			// assert
			Assert.True(wasEmptyBeforeAct);
			Assert.IsAssignableFrom<Resource>(resource);
			Assert.IsType<List<ResourceLink>>(resource.Links);
			Assert.Contains(resourceLink, resource.Links);
		}

		[Theory]
		[EnumerateToResourceData]
		[Trait(nameof(IResourceFactory), nameof(ResourceFactory.EnumerateToResources))]
		public void ReturnsIEnumerable__Resource__FromCalling_EnumerateToResources<T>(T source) where T : class
		{
			// arrange
			var resourceLink = GetResourceLinkFromMockArrangements<T>();

			// act
			var resources =
				(_sut as ResourceFactory)?.EnumerateToResources(source as IEnumerable, typeof(T)).ToArray();

			// assert
			Assert.IsAssignableFrom<IEnumerable<Resource>>(resources);
			Assert.NotEmpty(resources);
			Assert.True(resources.All(x => x.Links.All(l => l == resourceLink)));
		}

		ResourceLink GetResourceLinkFromMockArrangements<T>() where T : class
		{
			// creating additional stubs
			const string routeName = "test-route";
			var resourceLink = new ResourceLink(routeName, GetDummyUrl(routeName), GetDummyMethod());

			//mocking dependency methods
			var mockHateoasLink = new Mock<IHateoasLink<T>>();
			mockHateoasLink.Setup(x => x.RouteName).Returns(routeName);
			mockHateoasLink.Setup(x => x.GetRouteDictionary(It.IsAny<object>()))
			               .Returns(It.IsAny<IDictionary<string, object>>());

			_mockHateoasContext
				.Setup(x => x.GetApplicableLinks(It.IsAny<Type>(), It.IsAny<object>()))
				.Returns(new List<IHateoasLink> {mockHateoasLink.Object});

			_mockResourceLinkFactory
				.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
				.Returns(resourceLink);

			return resourceLink;
		}

		string GetDummyMethod() => new[] {"GET", "POST", "PUT", "PATCH", "DELETE"}[new Random().Next(0, 4)];

		string GetDummyUrl(string routeName) => $"http://hateoasnet.api/{routeName}";
	}
}
