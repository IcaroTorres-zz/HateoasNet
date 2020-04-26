using HateoasNet.Abstractions;
using HateoasNet.Configurations;
using HateoasNet.TestingObjects;
using HateoasNet.Tests.Configurations.HateoasContextTests;
using Xunit;
using Xunit.Abstractions;

namespace HateoasNet.Tests.Configurations.HateoasLinkTests
{
	[Collection(nameof(IHateoasLink))]
	public class HateoasLinkShould
	{
		public HateoasLinkShould(HateoasLinkFixture fixture, ITestOutputHelper outputHelper)
		{
			_fixture = fixture;
			_outputHelper = outputHelper;
		}

		private readonly HateoasLinkFixture _fixture;
		private readonly ITestOutputHelper _outputHelper;

		[Fact]
		[Trait(nameof(IHateoasLink), "Instantiation")]
		public void BeOfType_HateoasLink__Testee()
		{
			Assert.IsAssignableFrom<IHateoasLink>(_fixture.Sut);
			Assert.IsAssignableFrom<IHateoasLink<Testee>>(_fixture.Sut);
			Assert.IsType<HateoasLink<Testee>>(_fixture.Sut);
		}

		[Fact]
		[Trait(nameof(IHateoasLink), "Instantiation")]
		public void HaveNotNullValues_For_RouteName_And_RouteDictionaryFunction_And_PredicateFunction()
		{
			// assert
			Assert.NotNull(_fixture.Sut.RouteName);
			Assert.NotNull(_fixture.Sut.RouteDictionaryFunction);
			Assert.NotNull(_fixture.Sut.PredicateFunction);
		}

		[Fact]
		[Trait(nameof(IHateoasLink), "HasRouteData")]
		[Trait(nameof(IHateoasLink), "RouteDictionaryFunction")]
		public void ReturnIHateoasLink_FromCalling_HasRouteData_SettingNotNullValue_For_RouteDictionaryFunction()
		{
			// act
			var hateoasLink = _fixture.Sut.HasRouteData(x => new {id = x.LongIntegerValue, label = x.StringValue});

			// assert
			Assert.Same(_fixture.Sut, hateoasLink);
			Assert.NotNull(_fixture.Sut.RouteDictionaryFunction);
		}

		[Theory]
		[ConfigureData]
		[Trait(nameof(IHateoasLink), "GetRouteDictionary")]
		[Trait(nameof(IHateoasLink), "HasRouteData")]
		public void ReturnDictionary_FromCalling_GetRouteDictionary_Matching_ManuallyGeneratedDictionary(Testee testee)
		{
			// act
			var hateoasLink = _fixture.Sut.HasRouteData(x => new {id = x.LongIntegerValue, label = x.StringValue});
			var expected = new {id = testee.LongIntegerValue, label = testee.StringValue}.ToRouteDictionary();
			var actual = _fixture.Sut.GetRouteDictionary(testee);

			// assert
			Assert.Same(_fixture.Sut, hateoasLink);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[ConfigureData]
		[Trait(nameof(IHateoasLink), "IsApplicable")]
		[Trait(nameof(IHateoasLink), "HasConditional")]
		public void ReturnsBool_FromCalling_IsApplicable_EqualsTo_ManuallyGeneratedBool(Testee testee)
		{
			// act
			var hateoasLink = _fixture.Sut.HasConditional(x => x.BoolValue);

			// assert
			Assert.Same(_fixture.Sut, hateoasLink);
			Assert.Equal(testee.BoolValue, _fixture.Sut.IsApplicable(testee));

			// act
			_fixture.Sut.HasConditional(x => x.DecimalValue > 2000m);

			// assert
			Assert.Equal(testee.DecimalValue > 2000m, _fixture.Sut.IsApplicable(testee));

			// act
			_fixture.Sut.HasConditional(x => !string.IsNullOrWhiteSpace(x.StringValue));

			// assert
			Assert.Equal(!string.IsNullOrWhiteSpace(testee.StringValue), _fixture.Sut.IsApplicable(testee));

			// act
			_fixture.Sut.HasConditional(x => x.DecimalValue == new decimal(x.FloatValue));

			// assert
			Assert.Equal(testee.DecimalValue == new decimal(testee.FloatValue), _fixture.Sut.IsApplicable(testee));
		}
	}
}
