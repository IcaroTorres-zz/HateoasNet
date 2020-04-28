using HateoasNet.Abstractions;
using HateoasNet.Factories;
using Moq;
using Xunit;
using HateoasNet.Resources;
using HateoasNet.Configurations;
using HateoasNet.Tests.TestHelpers;
using System;
using JetBrains.Annotations;

namespace HateoasNet.Tests.Factories.ResourceLinkFactoryTests
{
    public class ResourceLinkFactoryShould
    {
        public ResourceLinkFactoryShould()
        {
            _mockUrlBuilder = new Mock<IUrlBuilder>();
            _mockHttpMethodFinder = new Mock<IHttpMethodFinder>();
            _sut = new ResourceLinkFactory(_mockUrlBuilder.Object, _mockHttpMethodFinder.Object);
        }

        private readonly Mock<IUrlBuilder> _mockUrlBuilder;
        private readonly Mock<IHttpMethodFinder> _mockHttpMethodFinder;
        private readonly IResourceLinkFactory _sut;

        [Theory]
        [CreateResourceLinkData]
        [Trait(nameof(IResourceLinkFactory), "Create")]
        public void ReturnsResourceLink_FromCalling_Create([CanBeNull] object routeData, string routeName, string url, string method)
        {
            // arrange
            var routeDictionary = routeData?.ToRouteDictionary();
            _mockUrlBuilder.Setup(x => x.Build(routeName, routeDictionary)).Returns(url);
            _mockHttpMethodFinder.Setup(x => x.Find(routeName)).Returns(method);

            // act
            var resourceLink = _sut.Create(routeName, routeDictionary);

            // assert
            Assert.IsType<ResourceLink>(resourceLink);
            Assert.NotNull(resourceLink);
            Assert.NotNull(resourceLink.Href);
            Assert.NotNull(resourceLink.Rel);
            Assert.NotNull(resourceLink.Method);
            Assert.Equal(url, resourceLink.Href);
            Assert.Equal(routeName, resourceLink.Rel);
            Assert.Equal(method, resourceLink.Method);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [Trait(nameof(IResourceLinkFactory), "Create")]
        [Trait(nameof(IResourceLinkFactory), "Exceptions")]
        public void Throws_ArgumentNullException_FromCalling_Create_Using_routeName_NullOrWhiteSpace(string routeName)
        {
             Assert.Throws<ArgumentNullException>("routeName", () => _sut.Create(routeName, null));
        }
    }
}
