using System;
using HateoasNet.Abstractions;
using HateoasNet.Configurations;
using HateoasNet.TestingObjects;
using Xunit;
using Xunit.Abstractions;

namespace HateoasNet.Tests.Configurations.HateoasResourceTests
{
	public class HateoasResourceThrows
	{
		public HateoasResourceThrows(ITestOutputHelper outputHelper)
		{
			_sut = new HateoasContext().GetOrInsert<Testee>();
			_outputHelper = outputHelper;
		}

		private readonly IHateoasResource<Testee> _sut;
		private readonly ITestOutputHelper _outputHelper;

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[Trait(nameof(IHateoasResource), "HasLink")]
		[Trait(nameof(IHateoasResource), "Exceptions")]
		public void ArgumentNullException_On_HasLink(string routeName)
		{
			Assert.Throws<ArgumentNullException>("routeName", () => _sut.HasLink(routeName));
		}
	}
}
