using System.Collections.Generic;
using HateoasNet.Abstractions;
using HateoasNet.Configurations;
using HateoasNet.TestingObjects;
using Xunit;
using Xunit.Abstractions;

namespace HateoasNet.Tests.Configurations.HateoasContextTests
{
	public class HateoasContextShould
	{
		public HateoasContextShould(ITestOutputHelper outputHelper)
		{
			_sut = new HateoasContext();
			_outputHelper = outputHelper;
		}

		private readonly IHateoasContext _sut;
		private readonly ITestOutputHelper _outputHelper;

		[Fact]
		[Trait(nameof(IHateoasContext), "Instantiation")]
		public void BeOfType_HateoasContext()
		{
			Assert.IsAssignableFrom<IHateoasContext>(_sut);
			Assert.IsType<HateoasContext>(_sut);
		}

		[Theory]
		[ConfigureData]
		[Trait(nameof(IHateoasContext), "HasResource")]
		public void ReturnFalse_FromCalling_HasResource_With_type_Invalid(object invalid)
		{
			Assert.False(_sut.HasResource(invalid.GetType()));
		}

		[Theory]
		[ConfigureData]
		[Trait(nameof(IHateoasContext), "HasResource")]
		public void ReturnTrue_FromCalling_HasResource_With_type_Valid<T>(T valid) where T : Testee
		{
			// act
			var hateoasContext = _sut.Configure<T>(resource => { });

			// assert
			Assert.Same(_sut, hateoasContext);
			Assert.True(_sut.HasResource(valid.GetType()));
		}

		[Theory]
		[ConfigureData]
		[Trait(nameof(IHateoasContext), "GetOrInsert")]
		[Trait(nameof(IHateoasContext), "HasResource")]
		public void ReturnIHateoasResource_FromCalling_GetOrInsert<T>(T testee) where T : Testee
		{
			// assert
			var stronglyTypedHateoasResource = (_sut as HateoasContext)?.GetOrInsert<T>();
			var interfaceHateoasResource = (_sut as HateoasContext)?.GetOrInsert(testee.GetType());

			// assert
			Assert.NotNull(stronglyTypedHateoasResource);
			Assert.NotNull(interfaceHateoasResource);
			Assert.Same(stronglyTypedHateoasResource, interfaceHateoasResource);
			Assert.True(_sut.HasResource(testee.GetType()));
			Assert.True(_sut.HasResource(typeof(T)));
		}

		[Theory]
		[ConfigureData]
		[Trait(nameof(IHateoasContext), "GetApplicableLinks")]
		public void ReturnEmptyList_FromCalling_GetApplicableLinks_With_type_Invalid(Testee invalid)
		{
			// act
			var hateoasLinks = _sut.GetApplicableLinks(invalid.GetType(), invalid);

			// assert
			Assert.IsType<List<IHateoasLink>>(hateoasLinks);
			Assert.Empty(hateoasLinks);
		}

		[Theory]
		[ConfigureData]
		[Trait(nameof(IHateoasContext), "Configure")]
		[Trait(nameof(IHateoasContext), "GetApplicableLinks")]
		public void ReturnList_FromCalling_GetApplicableLinks_With_type_Valid<T>(T valid) where T : Testee
		{
			// act
			var hateoasContext = _sut.Configure<T>(resource =>
			{
				resource.HasLink(valid.StringValue)
				        .HasConditional(x => x.BoolValue)
				        .HasRouteData(x => new {id = x.LongIntegerValue});
			});

			// assert
			Assert.Same(_sut, hateoasContext);
			AssertNotEmptyHateoasLinks(valid);
		}

		[Theory]
		[ConfigurationData]
		[Trait(nameof(IHateoasContext), "ApplyConfiguration")]
		[Trait(nameof(IHateoasContext), "GetApplicableLinks")]
		public void ReturnList_FromCalling_GetApplicableLinks_With_type_Valid_Using_ApplyConfiguration<T>(
			IHateoasResourceConfiguration<T> config, T testee)
			where T : Testee
		{
			// act
			var hateoasContext = _sut.ApplyConfiguration(config);

			// assert
			Assert.Same(_sut, hateoasContext);
			AssertNotEmptyHateoasLinks(testee);
		}

		[Theory]
		[ConfigurationData]
		[Trait(nameof(IHateoasContext), "ConfigureFromAssembly")]
		[Trait(nameof(IHateoasContext), "GetApplicableLinks")]
		public void ReturnList_FromCalling_GetApplicableLinks_With_type_Valid_Using_ConfigureFromAssembly<T>(
			IHateoasResourceConfiguration<T> testeeConfiguration, T testee)
			where T : Testee
		{
			// act
			var hateoasContext = _sut.ConfigureFromAssembly(testeeConfiguration.GetType().Assembly);

			// assert
			Assert.Same(_sut, hateoasContext);
			AssertNotEmptyHateoasLinks(testee);
		}

		private void AssertNotEmptyHateoasLinks<T>(T data) where T : Testee
		{
			// act
			var hateoasLinks = _sut.GetApplicableLinks(typeof(T), data);

			// assert
			Assert.IsType<List<IHateoasLink>>(hateoasLinks);
			Assert.NotEmpty(hateoasLinks);
		}
	}
}
