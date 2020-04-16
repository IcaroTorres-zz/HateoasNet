using System;
using System.Collections.Generic;
using System.Linq;
using HateoasNet.Abstractions;
using HateoasNet.Framework.Mapping;
using HateoasNet.Mapping;
using HateoasNet.TestingObjects;
using Xunit;

namespace HateoasNet.Framework.Tests.Mapping
{
	public class HateoasMapTest
	{
		private readonly IHateoasMap<TestObject> _sut;

		public HateoasMapTest()
		{
			_sut = new HateoasConfiguration().GetOrInsert<TestObject>();
		}

		[Fact]
		public void Be_HateoasMap__TestObject()
		{
			Assert.IsType<HateoasMap<TestObject>>(_sut);
		}

		[Fact]
		public void Have_HateoasLinks__TestObject()
		{
			Assert.IsType<List<IHateoasLink>>(_sut.GetLinks().ToList());
		}

		[Fact]
		public void Create_HateoasLink__TestObject__On_HasLink()
		{
			var hateoasLink = _sut.HasLink("test");
			
			Assert.NotNull(hateoasLink);
			Assert.IsType<HateoasLink<TestObject>>(hateoasLink);
		}

		[Fact]
		public void NotAllowNull_routeName_On_HasLink()
		{
			Assert.Throws<ArgumentNullException>("routeName", () => _sut.HasLink(null));
		}
	}
}