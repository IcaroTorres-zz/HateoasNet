using System;
using System.Collections.Generic;
using System.Reflection;
using HateoasNet.Abstractions;
using HateoasNet.Configurations;
using HateoasNet.TestingObjects;
using HateoasNet.Tests.TestHelpers;
using Xunit;

namespace HateoasNet.Tests.Configurations
{
	public class HateoasContextTests : IDisposable
	{
		public HateoasContextTests()
		{
			_sut = new HateoasContext();
		}

		/// <inheritdoc />
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		private readonly IHateoasContext _sut;

		[Theory]
		[ConfigureData]
		[Trait(nameof(IHateoasContext), nameof(IHateoasContext.HasResource))]
		public void HasResource_WithInvalidType_ReturnFalse(object invalid)
		{
			// act
			var actual = _sut.HasResource(invalid.GetType());

			// assert
			Assert.False(actual);
		}

		[Theory]
		[ConfigureData]
		[Trait(nameof(IHateoasContext), nameof(IHateoasContext.HasResource))]
		public void HasResource_WithValidType_ReturnTrue<T>(T valid) where T : class
		{
			// arrange
			var hateoasContext = _sut.Configure<T>(resource => { });

			// act
			var actual = _sut.HasResource(valid.GetType());

			// assert
			Assert.Same(_sut, hateoasContext);
			Assert.True(actual);
		}

		[Theory]
		[ConfigureData]
		[Trait(nameof(IHateoasContext), nameof(HateoasContext.GetOrInsert))]
		[Trait(nameof(IHateoasContext), nameof(IHateoasContext.HasResource))]
		public void GetOrInsert_WithClassType_ReturnIHateoasResource<T>(T instance) where T : class
		{
			// act
			var interfaceHateoasResource = (_sut as HateoasContext)?.GetOrInsert(instance.GetType());
			var stronglyTypedHateoasResource = (_sut as HateoasContext)?.GetOrInsert<T>();
			var actualGetType = _sut.HasResource(instance.GetType());
			var actualTypeof = _sut.HasResource(typeof(T));

			// assert
			Assert.NotNull(stronglyTypedHateoasResource);
			Assert.NotNull(interfaceHateoasResource);
			Assert.Same(stronglyTypedHateoasResource, interfaceHateoasResource);
			Assert.True(actualGetType);
			Assert.True(actualTypeof);
		}

		[Theory]
		[ConfigureData]
		[Trait(nameof(IHateoasContext), nameof(IHateoasContext.GetApplicableLinks))]
		public void GetApplicableLinks_WithNotConfiguredType_ReturnEmptyList(Testee instance)
		{
			// arrange
			var notConfiguredType = instance.GetType();

			// act
			var hateoasLinks = _sut.GetApplicableLinks(notConfiguredType, instance);

			// assert
			Assert.IsType<List<IHateoasLink>>(hateoasLinks);
			Assert.Empty(hateoasLinks);
		}

		[Theory]
		[ConfigureData]
		[Trait(nameof(IHateoasContext), nameof(IHateoasContext.Configure))]
		[Trait(nameof(IHateoasContext), nameof(IHateoasContext.GetApplicableLinks))]
		public void GetApplicableLinks_WithConfiguredType_ReturnNotEmptyList<T>(T valid) where T : Testee
		{
			// arrange
			var hateoasContext = _sut.Configure<T>(resource =>
			{
				resource.HasLink(valid.StringValue)
				        .HasConditional(x => x.BoolValue)
				        .HasRouteData(x => new {id = x.LongIntegerValue});
			});

			// assert
			Assert.Same(_sut, hateoasContext);
			ActNotEmptyHateoasLinks(valid);
		}

		[Theory]
		[ConfigurationData]
		[Trait(nameof(IHateoasContext), nameof(IHateoasContext.ApplyConfiguration))]
		[Trait(nameof(IHateoasContext), nameof(IHateoasContext.GetApplicableLinks))]
		public void GetApplicableLinks_WithApplyConfigurationForType_ReturnNotEmptyList<T>(
			IHateoasResourceConfiguration<T> config, T testee)
			where T : Testee
		{
			// act
			var hateoasContext = _sut.ApplyConfiguration(config);
			ActNotEmptyHateoasLinks(testee);

			// assert
			Assert.Same(_sut, hateoasContext);
		}

		[Theory]
		[ConfigurationData]
		[Trait(nameof(IHateoasContext), nameof(IHateoasContext.ConfigureFromAssembly))]
		[Trait(nameof(IHateoasContext), nameof(IHateoasContext.GetApplicableLinks))]
		public void GetApplicableLinks_WithTypeConfiguredFromAssembly_ReturnNotEmptyList<T>(
			IHateoasResourceConfiguration<T> testeeConfiguration, T testee)
			where T : Testee
		{
			// act
			var hateoasContext = _sut.ConfigureFromAssembly(testeeConfiguration.GetType().Assembly);

			// assert
			Assert.Same(_sut, hateoasContext);
			ActNotEmptyHateoasLinks(testee);
		}

		private void ActNotEmptyHateoasLinks<T>(T data) where T : Testee
		{
			// act
			var hateoasLinks = _sut.GetApplicableLinks(typeof(T), data);

			// assert
			Assert.IsType<List<IHateoasLink>>(hateoasLinks);
			Assert.NotEmpty(hateoasLinks);
		}

		[Fact]
		[Trait(nameof(IHateoasContext), nameof(IHateoasContext.ApplyConfiguration))]
		[Trait(nameof(IHateoasContext), "Exceptions")]
		public void ApplyConfiguration_WithResourceNull_ThrowsArgumentNullException()
		{
			// arrange
			const string parameterName = "configuration";

			// act
			Action actual = () => _sut.ApplyConfiguration<Testee>(null);

			Assert.Throws<ArgumentNullException>(parameterName, actual);
		}

		[Fact]
		[Trait(nameof(IHateoasContext), nameof(IHateoasContext.Configure))]
		[Trait(nameof(IHateoasContext), "Exceptions")]
		public void Configure_WithResourceNull_ThrowsArgumentNullException()
		{
			// arrange
			const string parameterName = "resource";

			// act
			Action actual = () => _sut.Configure<Testee>(null);

			// assert
			Assert.Throws<ArgumentNullException>(parameterName, actual);
		}

		[Fact]
		[Trait(nameof(IHateoasContext), nameof(IHateoasContext.ConfigureFromAssembly))]
		[Trait(nameof(IHateoasContext), "Exceptions")]
		public void ConfigureFromAssembly_WhenHasNo_IHateoasResourceConfiguration_ThrowsTargetException()
		{
			// act
			Action actual = () => _sut.ConfigureFromAssembly(typeof(string).Assembly);

			// assert
			Assert.Throws<TargetException>(actual);
		}

		[Fact]
		[Trait(nameof(IHateoasContext), nameof(IHateoasContext.ConfigureFromAssembly))]
		[Trait(nameof(IHateoasContext), "Exceptions")]
		public void ConfigureFromAssembly_WithResourceNull_ThrowsArgumentNullException()
		{
			// arrange
			const string parameterName = "assembly";

			// act
			Action actual = () => _sut.ConfigureFromAssembly(null);

			// assert
			Assert.Throws<ArgumentNullException>(parameterName, actual);
		}

		[Fact]
		[Trait(nameof(IHateoasContext), "New")]
		public void New_WithOutParameters_HateoasContext()
		{
			Assert.IsAssignableFrom<IHateoasContext>(_sut);
			Assert.IsType<HateoasContext>(_sut);
		}
	}
}
