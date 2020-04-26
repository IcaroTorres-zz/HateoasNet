using System;
using HateoasNet.Abstractions;
using Xunit;
using Xunit.Abstractions;

namespace HateoasNet.Tests.Configurations.HateoasLinkTests
{
	[Collection(nameof(IHateoasLink))]
	public class HateoasLinkThrows
	{
		public HateoasLinkThrows(HateoasLinkFixture fixture, ITestOutputHelper outputHelper)
		{
			_fixture = fixture;
			_outputHelper = outputHelper;
		}

		private readonly HateoasLinkFixture _fixture;
		private readonly ITestOutputHelper _outputHelper;

		[Fact]
		[Trait(nameof(IHateoasLink), "GetRouteDictionary")]
		[Trait(nameof(IHateoasLink), "Exceptions")]
		public void ArgumentNullException_On_GetRouteDictionary()
		{
			Assert.Throws<ArgumentNullException>("resourceData", () => _fixture.Sut.GetRouteDictionary(null));
		}

		[Fact]
		[Trait(nameof(IHateoasLink), "HasConditional")]
		[Trait(nameof(IHateoasLink), "Exceptions")]
		public void ArgumentNullException_On_HasConditional()
		{
			Assert.Throws<ArgumentNullException>("predicate", () => _fixture.Sut.HasConditional(null));
		}

		[Fact]
		[Trait(nameof(IHateoasLink), "HasRouteData")]
		[Trait(nameof(IHateoasLink), "Exceptions")]
		public void ArgumentNullException_On_HasRouteData()
		{
			Assert.Throws<ArgumentNullException>("routeDataFunction", () => _fixture.Sut.HasRouteData(null));
		}

		[Fact]
		[Trait(nameof(IHateoasLink), "IsApplicable")]
		[Trait(nameof(IHateoasLink), "Exceptions")]
		public void ArgumentNullException_On_IsApplicable()
		{
			Assert.Throws<ArgumentNullException>("resourceData", () => _fixture.Sut.IsApplicable(null) as object);
		}
	}
}
