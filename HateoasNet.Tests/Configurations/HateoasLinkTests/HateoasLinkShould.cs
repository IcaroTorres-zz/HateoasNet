using System;
using HateoasNet.Abstractions;
using HateoasNet.Configurations;
using HateoasNet.TestingObjects;
using HateoasNet.Tests.TestHelpers;
using Xunit;

namespace HateoasNet.Tests.Configurations.HateoasLinkTests
{
	[Collection(nameof(IHateoasLink))]
	public class HateoasLinkShould
	{
		public HateoasLinkShould(HateoasLinkFixture fixture)
		{
			_fixture = fixture;
		}

		private readonly HateoasLinkFixture _fixture;

		[Theory]
		[HasConditionalData]
		[Trait(nameof(IHateoasLink), nameof(IHateoasLink.IsApplicable))]
		[Trait(nameof(IHateoasLink), nameof(IHateoasLink<Testee>.HasConditional))]
		public void IsApplicable_And_HasConditionalParameterFunction_ReturnsSameValue<T>(T testee, Func<T, bool> function)
			where T : Testee
		{
			// arrange
			var newSut = new HateoasLink<T>(typeof(T).Name);
			var expected = function(testee);
			var hateoasLink = newSut.HasConditional(function);

			// act
			var actual = newSut.IsApplicable(testee);

			// assert
			Assert.Same(newSut, hateoasLink);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[ConfigureData]
		[Trait(nameof(IHateoasLink), nameof(IHateoasLink<Testee>.GetRouteDictionary))]
		[Trait(nameof(IHateoasLink), nameof(IHateoasLink<Testee>.HasRouteData))]
		public void GetRouteDictionary_And_HasRouteDataParameterFunction_ReturnsSameValue(Testee testee)
		{
			// act
			var hateoasLink = _fixture.Sut.HasRouteData(x => new {id = x.LongIntegerValue, label = x.StringValue});
			var expected = new {id = testee.LongIntegerValue, label = testee.StringValue}.ToRouteDictionary();
			var actual = _fixture.Sut.GetRouteDictionary(testee);

			// assert
			Assert.Same(_fixture.Sut, hateoasLink);
			Assert.Equal(expected, actual);
		}


		[Fact]
		[Trait(nameof(IHateoasLink), nameof(IHateoasLink<Testee>.GetRouteDictionary))]
		[Trait(nameof(IHateoasLink), "Exceptions")]
		public void GetRouteDictionary_WithResourceDataNull_ThrowsArgumentNullException()
		{
			// arrange
			const string parameterName = "resourceData";

			// act
			Action actual = () => _fixture.Sut.GetRouteDictionary(null);

			// assert
			Assert.Throws<ArgumentNullException>(parameterName, actual);
		}

		[Fact]
		[Trait(nameof(IHateoasLink), nameof(IHateoasLink<Testee>.HasConditional))]
		[Trait(nameof(IHateoasLink), "Exceptions")]
		public void HasConditional_WithPredicateNull_ThrowsArgumentNullException()
		{
			// arrange
			const string parameterName = "predicate";

			// act
			Action actual = () => _fixture.Sut.HasConditional(null);

			// assert
			Assert.Throws<ArgumentNullException>(parameterName, actual);
		}

		[Fact]
		[Trait(nameof(IHateoasLink), nameof(IHateoasLink<Testee>.HasRouteData))]
		[Trait(nameof(IHateoasLink), "Exceptions")]
		public void HasRouteData_WithRouteDataFunctionNull_ThrowsArgumentNullException()
		{
			// arrange
			const string parameterName = "routeDataFunction";

			// act
			Action actual = () => _fixture.Sut.HasRouteData(null);

			// assert
			Assert.Throws<ArgumentNullException>(parameterName, actual);
		}

		[Fact]
		[Trait(nameof(IHateoasLink), nameof(IHateoasLink<Testee>.HasRouteData))]
		[Trait(nameof(IHateoasLink), nameof(IHateoasLink<Testee>.RouteDictionaryFunction))]
		public void HasRouteData_WithValidRouteDataFunction_ReturnIHateoasLink()
		{
			// act
			var hateoasLink = _fixture.Sut.HasRouteData(x => new {id = x.LongIntegerValue, label = x.StringValue});

			// assert
			Assert.Same(_fixture.Sut, hateoasLink);
			Assert.NotNull(_fixture.Sut.RouteDictionaryFunction);
		}

		[Fact]
		[Trait(nameof(IHateoasLink), "New")]
		public void HaveNotNullValues_For_RouteName_And_RouteDictionaryFunction_And_PredicateFunction()
		{
			// assert
			Assert.NotNull(_fixture.Sut.RouteName);
			Assert.NotNull(_fixture.Sut.RouteDictionaryFunction);
			Assert.NotNull(_fixture.Sut.Predicate);
		}

		[Fact]
		[Trait(nameof(IHateoasLink), nameof(IHateoasLink.IsApplicable))]
		[Trait(nameof(IHateoasLink), "Exceptions")]
		public void IsApplicable_WithResourceDataNull_ThrowsArgumentNullException()
		{
			// arrange
			const string parameterName = "resourceData";

			// act
			Action actual = () => _fixture.Sut.IsApplicable(null);

			// assert
			Assert.Throws<ArgumentNullException>(parameterName, actual);
		}

		[Fact]
		[Trait(nameof(IHateoasLink), "New")]
		public void New_WithValidParameters_ReturnsHateoasLink()
		{
			Assert.IsAssignableFrom<IHateoasLink>(_fixture.Sut);
			Assert.IsAssignableFrom<IHateoasLink<Testee>>(_fixture.Sut);
			Assert.IsType<HateoasLink<Testee>>(_fixture.Sut);
		}
	}
}
