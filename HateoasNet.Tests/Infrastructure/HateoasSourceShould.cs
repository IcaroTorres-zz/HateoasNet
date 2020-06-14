using FluentAssertions;
using HateoasNet.Abstractions;
using HateoasNet.Infrastructure;
using System;
using System.Collections.Generic;
using Xunit;

namespace HateoasNet.Tests.Infrastructure
{
    public class HateoasSourceShould : IDisposable
    {
        private readonly HateoasSource<HateoasSample> _sut;

        public HateoasSourceShould()
        {
            _sut = new HateoasSource<HateoasSample>();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        [Fact]
        [Trait(nameof(IHateoasSource), "Instance")]
        public void New_WithTypeParameter_CreatesHateoasSource()
        {
            _sut.Should()
                .BeAssignableTo<IHateoasSource>().And
                .BeAssignableTo<IHateoasSource<HateoasSample>>().And
                .BeOfType<HateoasSource<HateoasSample>>();
        }

        [Fact]
        [Trait(nameof(IHateoasSource), nameof(IHateoasSource.GetLinkBuilders))]
        public void GetLinks_FromHateoasSource_WithOutConfiguredLinks_ReturnsEmptyLinkBuilders()
        {
            _sut.GetLinkBuilders().Should().BeAssignableTo<IEnumerable<IHateoasLinkBuilder>>().And.BeEmpty();
        }

        [Fact]
        [Trait(nameof(IHateoasSource), nameof(IHateoasSource<HateoasSample>.AddLink))]
        public void HasLink_WithNotEmptyString_ReturnsHateoasLinkBuilder()
        {
            _sut.AddLink("not empty string").Should()
                .NotBeNull().And
                .BeAssignableTo<IHateoasLinkBuilder>().And
                .BeAssignableTo<IHateoasLinkBuilder<HateoasSample>>().And
                .BeOfType<HateoasLinkBuilder<HateoasSample>>();
        }

        [Fact]
        [Trait(nameof(IHateoasSource), nameof(IHateoasSource.GetLinkBuilders))]
        public void GetLinks_FromHateoasSource_WithConfiguredLinks_ReturnsLinkBuilders()
        {
            // arrange
            var linkBuilder = _sut.AddLink("not empty string").Should()
                .BeAssignableTo<IHateoasLinkBuilder>().And
                .BeAssignableTo<IHateoasLinkBuilder<HateoasSample>>().And
                .BeOfType<HateoasLinkBuilder<HateoasSample>>().Subject;

            _sut.GetLinkBuilders().Should()
                .BeAssignableTo<IEnumerable<IHateoasLinkBuilder>>().And
                .Contain(linkBuilder);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [Trait(nameof(IHateoasSource), nameof(IHateoasSource<HateoasSample>.AddLink))]
        [Trait(nameof(IHateoasSource), "Exceptions")]
        public void HasLink_WithRouteNameNullOrEmpty_ThrowsArgumentNullException(string routeName)
        {
            // arrange
            const string parameterName = "routeName";

            // act
            Action actual = () => _sut.AddLink(routeName);

            Assert.Throws<ArgumentNullException>(parameterName, actual);
        }
    }
}
