using System;
using System.Reflection;
using HateoasNet.Abstractions;
using HateoasNet.Configurations;
using HateoasNet.TestingObjects;
using Xunit;
using Xunit.Abstractions;

namespace HateoasNet.Tests.Configurations.HateoasContextTests
{
	public class HateoasContextThrows
	{
		public HateoasContextThrows(ITestOutputHelper outputHelper)
		{
			_sut = new HateoasContext();
			_outputHelper = outputHelper;
		}

		private readonly IHateoasContext _sut;
		private readonly ITestOutputHelper _outputHelper;

		[Fact]
		[Trait(nameof(IHateoasContext), "Configure")]
		[Trait(nameof(IHateoasContext), "Exceptions")]
		public void ArgumentNullException_On_Configure()
		{
			Assert.Throws<ArgumentNullException>("resource", () => _sut.Configure<Testee>(null));
		}

		[Fact]
		[Trait(nameof(IHateoasContext), "ApplyConfiguration")]
		[Trait(nameof(IHateoasContext), "Exceptions")]
		public void ArgumentNullException_On_ApplyConfiguration()
		{
			Assert.Throws<ArgumentNullException>("configuration", () => _sut.ApplyConfiguration<Testee>(null));
		}

		[Fact]
		[Trait(nameof(IHateoasContext), "ConfigureFromAssembly")]
		[Trait(nameof(IHateoasContext), "Exceptions")]
		public void ArgumentNullException_On_ConfigureFromAssembly()
		{
			Assert.Throws<ArgumentNullException>("assembly", () => _sut.ConfigureFromAssembly(null));
		}

		[Fact]
		[Trait(nameof(IHateoasContext), "ConfigureFromAssembly")]
		[Trait(nameof(IHateoasContext), "Exceptions")]
		public void TargetException_On_ConfigureFromAssembly_WhenHasNo_IHateoasResourceConfiguration()
		{
			Assert.Throws<TargetException>(() => _sut.ConfigureFromAssembly(typeof(string).Assembly));
		}
	}
}
