using HateoasNet.Abstractions;
using HateoasNet.Configurations;
using HateoasNet.TestingObjects;
using HateoasNet.Tests.TestHelpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace HateoasNet.Tests.Configurations
{
    public class HateoasResourceTests : IDisposable
    {
        [Theory]
        [ConfigureData]
        [Trait(nameof(IHateoasResource), "New")]
        public void New_WithTypeParameter_CreatesHateoasResource<T>(T testee) where T : Testee
        {
            // act
            var sut = new HateoasResource<T>();

            // assert
            Assert.IsAssignableFrom<IHateoasResource>(sut);
            Assert.IsAssignableFrom<IHateoasResource<T>>(sut);
            Assert.IsType<HateoasResource<T>>(sut);
        }

        [Theory]
        [ConfigureData]
        [Trait(nameof(IHateoasResource), nameof(IHateoasResource<Testee>.HasLink))]
        public void HasLink_WithNotEmptyString_ReturnsHateoasLink<T>(T testee) where T : class
        {
            // act
            var sut = new HateoasResource<T>();
            var hateoasLink = sut.HasLink(testee.GetType().Name);

            // assert
            Assert.NotNull(hateoasLink);
            Assert.IsAssignableFrom<IHateoasLink>(hateoasLink);
            Assert.IsAssignableFrom<IHateoasLink<T>>(hateoasLink);
            Assert.IsType<HateoasLink<T>>(hateoasLink);
        }

        [Theory]
        [ConfigureData]
        [Trait(nameof(IHateoasResource), "GetLinks")]
        public void GetLinks_FromHateoasResource_WithOutConfiguredLinks_ReturnsEmptyLinks<T>(T _) where T : Testee
        {
            // arrange
            var sut = new HateoasResource<T>();

            // act
            var hateoasLinks = sut.GetLinks();

            // assert
            Assert.IsAssignableFrom<IEnumerable<IHateoasLink>>(hateoasLinks);
            Assert.IsType<List<IHateoasLink>>(hateoasLinks);
            Assert.Empty(hateoasLinks);
        }

        [Theory]
        [ConfigureData]
        [Trait(nameof(IHateoasResource), nameof(IHateoasResource<Testee>.GetLinks))]
        public void GetLinks_FromHateoasResource_WithConfiguredLinks_ReturnsNotEmptyLinks<T>(T _) where T : Testee
        {
            // arrange
            const string routeName = "test";
            var sut = new HateoasResource<T>();

            // act
            var hateoasLink = sut.HasLink(routeName);
            var hateoasLinks = sut.GetLinks();

            // assert
            Assert.IsAssignableFrom<IEnumerable<IHateoasLink>>(hateoasLinks);
            Assert.IsType<List<IHateoasLink>>(hateoasLinks);
            Assert.IsAssignableFrom<IHateoasLink>(hateoasLink);
            Assert.IsAssignableFrom<IHateoasLink<T>>(hateoasLink);
            Assert.IsType<HateoasLink<T>>(hateoasLink);
            Assert.Contains(hateoasLinks, x => x.Equals(hateoasLink));
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [Trait(nameof(IHateoasResource), nameof(IHateoasResource<Testee>.HasLink))]
        [Trait(nameof(IHateoasResource), "Exceptions")]
        public void HasLink_WithRouteNameNullOrEmpty_ThrowsArgumentNullException(string routeName)
        {
            // arrange
            var sut = new HateoasResource<Testee>();
            const string parameterName = "routeName";

            // act
            Action actual = () => sut.HasLink(routeName);

            Assert.Throws<ArgumentNullException>(parameterName, actual);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
