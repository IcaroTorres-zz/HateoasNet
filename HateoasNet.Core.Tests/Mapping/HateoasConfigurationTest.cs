using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HateoasNet.Abstractions;
using HateoasNet.Core.Mapping;
using HateoasNet.TestingObjects;
using Xunit;

namespace HateoasNet.Core.Tests.Mapping
{
	public class HateoasConfigurationTest
	{
		private readonly IHateoasConfiguration _sut;

		public HateoasConfigurationTest()
		{
			_sut = new HateoasConfiguration();
		}

		[Fact]
		public void Be_HateoasConfiguration()
		{
			Assert.IsType<HateoasConfiguration>(_sut);
		}

		private void AssertHateoasLinks()
		{
			Assert.IsType<List<IHateoasLink>>(_sut.GetMappedLinks(typeof(TestObject), new TestObject()).ToList());
		}
		
		[Fact]
		public void Have_HateoasLinks__TestObject__On_GetMappedLinks_When_Map__TestObject()
		{
			// act
			_sut.Map<TestObject>(map => { });
			
			// assert
			AssertHateoasLinks();
		}
		
		[Fact]
		public void Have_HateoasLinks__TestObject__On_GetMappedLinks_When_ApplyConfiguration__TestObjectHateoas()
		{
			// act
			_sut.ApplyConfiguration(new TestObjectHateoas());
			
			// assert
			AssertHateoasLinks();
		}

		[Fact]
		public void Have_HateoasLinks__TestObject__On_GetMappedLinks_When_ApplyConfigurationFromAssembly()
		{
			// act
			_sut.ApplyConfigurationsFromAssembly(typeof(TestObjectHateoas).Assembly);

			// assert
			AssertHateoasLinks();
		}

		[Fact]
		public void NotAllowNull_mapper_On_Map__TestObject()
		{
			Assert.Throws<ArgumentNullException>("mapper", () => _sut.Map<TestObject>(null));
		}

		[Fact]
		public void NotAllowNull_builder_On_ApplyConfiguration__TestObject()
		{
			Assert.Throws<ArgumentNullException>("builder", () => _sut.ApplyConfiguration<TestObject>(null));
		}
		
		[Fact]
		public void NotAllowNull_assembly_On_ApplyConfigurationFromAssembly()
		{
			Assert.Throws<ArgumentNullException>("assembly", () => _sut.ApplyConfigurationsFromAssembly(null));
		}
		
		[Fact]
		public void NotAllow_assembly_WithNo_IHateoasBuilder_On_ApplyConfigurationFromAssembly()
		{
			Assert.Throws<TargetException>(() => _sut.ApplyConfigurationsFromAssembly(typeof(string).Assembly));
		}
	}
}