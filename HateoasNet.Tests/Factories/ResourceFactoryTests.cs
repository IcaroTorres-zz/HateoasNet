using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HateoasNet.Abstractions;
using HateoasNet.Factories;
using HateoasNet.Resources;
using HateoasNet.Tests.TestHelpers;
using Moq;
using Xunit;

namespace HateoasNet.Tests.Factories
{
	public class ResourceFactoryTests : IDisposable
	{
		private readonly Mock<IHateoasContext> _mockHateoasContext;
		private readonly Mock<IResourceLinkFactory> _mockResourceLinkFactory;
		private readonly IResourceFactory _sut;

		public ResourceFactoryTests()
		{
			_mockHateoasContext = new Mock<IHateoasContext>();
			_mockResourceLinkFactory = new Mock<IResourceLinkFactory>();
			_sut = new ResourceFactory(_mockHateoasContext.Object, _mockResourceLinkFactory.Object);
		}

		/// <inheritdoc />
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		[Theory]
		[CreateResourceData]
		[Trait(nameof(IResourceFactory), nameof(IResourceFactory.Create))]
		public void Create_WithValidParameters_ReturnsValidFormattedResource<T>(T source) where T : class
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
					innerLinks = enumerableResource.GetItems().SelectMany(x => x.Links).ToList();
					resource = enumerableResource;
					break;

				case IPagination pagination:
					var paginationResource = _sut.Create(pagination, typeof(T));
					innerLinks = paginationResource.GetItems().SelectMany(x => x.Links).ToList();
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
		[Trait(nameof(IResourceFactory), nameof(ResourceFactory.BuildResourceLinks))]
		public void BuildResourceLinks_WithValidParameters_AddsLinksToResource<T>(Resource resource, T source, Type type)
			where T : class

		{
			// arrange
			var wasEmptyBeforeAct = !resource.Links.Any();
			var resourceLink = GetResourceLinkFromMockArrangements<T>();

			// act
			((ResourceFactory) _sut).BuildResourceLinks(resource, source, type);

			// assert
			Assert.True(wasEmptyBeforeAct);
			Assert.IsAssignableFrom<Resource>(resource);
			Assert.IsType<List<ResourceLink>>(resource.Links);
			Assert.Contains(resourceLink, resource.Links);
		}

		[Theory]
		[EnumerateToResourceData]
		[Trait(nameof(IResourceFactory), nameof(ResourceFactory.ToEnumerableOfResources))]
		public void ToEnumerableOfResources_WithEnumerableOfTargetType_ReturnsIEnumerableOfSingleResource<T>(T source)
			where T : class
		{
			// arrange
			var resourceLink = GetResourceLinkFromMockArrangements<T>();

			// act
			var resources =
				((ResourceFactory) _sut).ToEnumerableOfResources(source as IEnumerable, typeof(T)).ToArray();

			// assert
			Assert.IsAssignableFrom<IEnumerable<Resource>>(resources);
			Assert.NotEmpty(resources);
			Assert.True(resources.All(x => x.Links.All(l => l == resourceLink)));
		}

		private ResourceLink GetResourceLinkFromMockArrangements<T>() where T : class
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

		private string GetDummyMethod()
		{
			return new[] {"GET", "POST", "PUT", "PATCH", "DELETE"}[new Random().Next(0, 4)];
		}

		private string GetDummyUrl(string routeName)
		{
			return $"http://hateoasnet.api/{routeName}";
		}
	}
}
