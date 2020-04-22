using System;
using System.Collections.Generic;
using System.Linq;
using HateoasNet.Abstractions;
using HateoasNet.Configurations;
using HateoasNet.TestingObjects;
using Xunit;

namespace HateoasNet.Framework.Tests.Configurations
{
	public class HateoasMapTest
	{
		public HateoasMapTest()
		{
			_sut = new HateoasContext().GetOrInsert<TestObject>();
		}

		private readonly IHateoasResource<TestObject> _sut;

		[Fact]
		public void Be_HateoasMap__TestObject()
		{
			Assert.IsType<HateoasResource<TestObject>>(_sut);
		}

		[Fact]
		public void Create_HateoasLink__TestObject__On_HasLink()
		{
			var hateoasLink = _sut.HasLink("test");

			Assert.NotNull(hateoasLink);
			Assert.IsType<HateoasLink<TestObject>>(hateoasLink);
		}

		[Fact]
		public void Have_HateoasLinks__TestObject()
		{
			Assert.IsType<List<IHateoasLink>>(_sut.GetLinks().ToList());
		}

		[Fact]
		public void NotAllowNull_routeName_On_HasLink()
		{
			Assert.Throws<ArgumentNullException>("routeName", () => _sut.HasLink(null));
		}
	}
}