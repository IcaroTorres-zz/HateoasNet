using System.Collections.Generic;
using HateoasNet.Abstractions;
using HateoasNet.Configurations;
using HateoasNet.TestingObjects;
using HateoasNet.Tests.Configurations.HateoasContextTests;
using Xunit;
using Xunit.Abstractions;

namespace HateoasNet.Tests.Configurations.HateoasResourceTests
{
	public class HateoasResourceShould
	{
		public HateoasResourceShould(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		private readonly ITestOutputHelper _outputHelper;

		[Theory]
		[ConfigureData]
		[Trait(nameof(IHateoasResource), "Instantiation")]
		public void BeOfType_HateoasResource<T>(T testee) where T : Testee
		{
			// act
			var sut = new HateoasResource<T>();

			Assert.IsAssignableFrom<IHateoasResource>(sut);
			Assert.IsAssignableFrom<IHateoasResource<T>>(sut);
			Assert.IsType<HateoasResource<T>>(sut);
		}

		[Theory]
		[ConfigureData]
		[Trait(nameof(IHateoasResource), "HasLink")]
		public void ReturnsHateoasLink_FromCalling_HasLink<T>(T testee) where T : Testee
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
		public void ReturnsEmptyList_FromCalling_GetLinks_IfNo_LinkConfigured<T>(T testee) where T : Testee
		{
			// act
			var sut = new HateoasResource<T>();
			var hateoasLinks = sut.GetLinks();

			// assert
			Assert.IsAssignableFrom<IEnumerable<IHateoasLink>>(hateoasLinks);
			Assert.IsType<List<IHateoasLink>>(hateoasLinks);
			Assert.Empty(hateoasLinks);
		}

		[Theory]
		[ConfigureData]
		[Trait(nameof(IHateoasResource), "GetLinks")]
		public void ReturnsList_FromCalling_GetLinks_IfAny_LinkConfigured<T>(T testee) where T : Testee
		{
			// act
			var sut = new HateoasResource<T>();
			var hateoasLink = sut.HasLink("test");
			var hateoasLinks = sut.GetLinks();

			// assert
			Assert.IsAssignableFrom<IEnumerable<IHateoasLink>>(hateoasLinks);
			Assert.IsType<List<IHateoasLink>>(hateoasLinks);

			Assert.IsAssignableFrom<IHateoasLink>(hateoasLink);
			Assert.IsAssignableFrom<IHateoasLink<T>>(hateoasLink);
			Assert.IsType<HateoasLink<T>>(hateoasLink);

			Assert.Contains(hateoasLinks, x => x.Equals(hateoasLink));
		}
	}
}
