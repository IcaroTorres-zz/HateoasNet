using FluentAssertions;
using HateoasNet.Abstractions;
using HateoasNet.Extensions;
using HateoasNet.Infrastructure;
using HateoasNet.Tests.TestHelpers;
using System;
using Xunit;

namespace HateoasNet.Tests.Infrastructure
{
    public class HateoasLinkBuilderShould : IDisposable
    {
        private readonly HateoasLinkBuilder<HateoasSample> _sut;
        public HateoasLinkBuilderShould()
        {
            _sut = new HateoasLinkBuilder<HateoasSample>("test");
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        [Fact]
        [Trait(nameof(IHateoasLinkBuilder), "Instance")]
        public void HaveNotNullValues_For_RouteName_And_RouteDictionaryFunction_And_PredicateFunction()
        {
            _sut.RouteName.Should().NotBeEmpty();
            _sut.RouteDictionaryFunction.Should().NotBeNull();
            _sut.Predicate.Should().NotBeNull();
        }

        [Fact]
        [Trait(nameof(IHateoasLinkBuilder), "Instance")]
        public void New_WithValidParameters_ReturnsHateoasLink()
        {
            _sut.Should()
                .BeAssignableTo<IHateoasLinkBuilder>().And
                .BeAssignableTo<IHateoasLinkBuilder<HateoasSample>>().And
                .BeOfType<HateoasLinkBuilder<HateoasSample>>();
        }

        [Fact]
        [Trait(nameof(IHateoasLinkBuilder), nameof(IHateoasLinkBuilder<HateoasSample>.HasRouteData))]
        [Trait(nameof(IHateoasLinkBuilder), nameof(IHateoasLinkBuilder<HateoasSample>.RouteDictionaryFunction))]
        public void HasRouteData_WithValidRouteDataFunction_ReturnIHateoasLink()
        {
            _sut.HasRouteData(x => new { id = x.Id, foreignKey = x.ForeignKeyId });

            _sut.RouteDictionaryFunction.Should().NotBeNull();
        }

        [Theory]
        [WhenData]
        [Trait(nameof(IHateoasLinkBuilder), nameof(IHateoasLinkBuilder.IsApplicable))]
        [Trait(nameof(IHateoasLinkBuilder), nameof(IHateoasLinkBuilder<HateoasSample>.When))]
        public void IsApplicable_PredicateFunction_ReturnsSameValue(HateoasSample data, Func<HateoasSample, bool> function)
        {
            _sut.When(function).IsApplicable(data).Should().Be(function(data));
        }

        [Fact]
        [Trait(nameof(IHateoasLinkBuilder), nameof(IHateoasLinkBuilder<HateoasSample>.PresentedAs))]
        public void PresentedAs_PresentedName_ReturnsSameValue()
        {
            const string expected = "new-name";
            _sut.PresentedAs(expected).PresentedName.Should().Be(expected);
        }

        [Fact]
        [Trait(nameof(IHateoasLinkBuilder), nameof(IHateoasLinkBuilder<HateoasSample>.GetRouteDictionary))]
        [Trait(nameof(IHateoasLinkBuilder), nameof(IHateoasLinkBuilder<HateoasSample>.HasRouteData))]
        public void GetRouteDictionary_HasRouteDataParameterFunction_ReturnsSameValue()
        {
            var data = new HateoasSample();
            var linkBuilder = _sut.HasRouteData(x => new { id = x.Id, foreignKey = x.ForeignKeyId });
            var expected = new { id = data.Id, foreignKey = data.ForeignKeyId }.ToRouteDictionary();

            _sut.GetRouteDictionary(data).Should().BeEquivalentTo(expected);
        }

        [Fact]
        [Trait(nameof(IHateoasLinkBuilder), nameof(IHateoasLinkBuilder<HateoasSample>.GetRouteDictionary))]
        [Trait(nameof(IHateoasLinkBuilder), "Exceptions")]
        public void GetRouteDictionary_WithResourceDataNull_ThrowsArgumentNullException()
        {
            // arrange
            const string parameterName = "source";

            // act
            Action actual = () => _sut.GetRouteDictionary(null);

            // assert
            Assert.Throws<ArgumentNullException>(parameterName, actual);
        }

        [Fact]
        [Trait(nameof(IHateoasLinkBuilder), nameof(IHateoasLinkBuilder<HateoasSample>.When))]
        [Trait(nameof(IHateoasLinkBuilder), "Exceptions")]
        public void HasConditional_WithPredicateNull_ThrowsArgumentNullException()
        {
            // arrange
            const string parameterName = "predicate";

            // act
            Action actual = () => _sut.When(null);

            // assert
            Assert.Throws<ArgumentNullException>(parameterName, actual);
        }

        [Fact]
        [Trait(nameof(IHateoasLinkBuilder), nameof(IHateoasLinkBuilder<HateoasSample>.HasRouteData))]
        [Trait(nameof(IHateoasLinkBuilder), "Exceptions")]
        public void HasRouteData_WithRouteDataFunctionNull_ThrowsArgumentNullException()
        {
            // arrange
            const string parameterName = "routeDataFunction";

            // act
            Action actual = () => _sut.HasRouteData(null);

            // assert
            Assert.Throws<ArgumentNullException>(parameterName, actual);
        }

        [Fact]
        [Trait(nameof(IHateoasLinkBuilder), nameof(IHateoasLinkBuilder.IsApplicable))]
        [Trait(nameof(IHateoasLinkBuilder), "Exceptions")]
        public void IsApplicable_WithResourceDataNull_ThrowsArgumentNullException()
        {
            // arrange
            const string parameterName = "source";

            // act
            Action actual = () => _sut.IsApplicable(null);

            // assert
            Assert.Throws<ArgumentNullException>(parameterName, actual);
        }
    }
}
