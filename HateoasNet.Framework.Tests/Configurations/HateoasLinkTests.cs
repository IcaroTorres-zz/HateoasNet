using System;
using System.Web.Routing;
using HateoasNet.Abstractions;
using HateoasNet.Configurations;
using HateoasNet.TestingObjects;
using Xunit;

namespace HateoasNet.Framework.Tests.Configurations
{
	public class HateoasLinkTests
	{
		public HateoasLinkTests()
		{
			_testObject = new TestObject {Value = "test-route"};
			_sut = new HateoasResource<TestObject>().HasLink(_testObject.Value.ToString());
		}

		private readonly IHateoasLink<TestObject> _sut;
		private readonly TestObject _testObject;

		[Fact]
		public void Be_HateoasLink__TestObject()
		{
			Assert.IsType<HateoasLink<TestObject>>(_sut);
		}

		[Fact]
		public void Have_RouteName_And_RouteDictionaryFunction_And_PredicateFunction()
		{
			// assert
			Assert.NotNull(_sut.RouteName);
			Assert.NotNull(_sut.RouteDictionaryFunction);
			Assert.NotNull(_sut.PredicateFunction);
		}

		[Fact]
		public void Match_Predicate_With_IsDisplayable_When_HasConditional()
		{
			// act
			_sut.HasConditional(x => x.Conditional);

			// assert
			Assert.Equal(_testObject.Conditional, _sut.IsApplicable(_testObject));
		}

		[Fact]
		public void Match_RouteValueDictionary_With_GetRouteDictionary_When_HasRouteData()
		{
			// act
			_sut.HasRouteData(x => new {id = x.Value.ToString()});
			var expected = new RouteValueDictionary(new {id = _testObject.Value.ToString()});
			var actual = _sut.GetRouteDictionary(_testObject);

			// assert
			Assert.Equal(expected.Keys, actual.Keys);
			Assert.Equal(expected.Values, actual.Values);
		}

		[Fact]
		public void NotAllowNull_predicate_On_HasConditional()
		{
			Assert.Throws<ArgumentNullException>("predicate", () => _sut.HasConditional(null));
		}

		[Fact]
		public void NotAllowNull_routeData_On_GetRouteDictionary()
		{
			Assert.Throws<ArgumentNullException>("resourceData", () => _sut.GetRouteDictionary(null));
		}

		[Fact]
		public void NotAllowNull_routeData_On_IsDisplayable()
		{
			Assert.Throws<ArgumentNullException>("resourceData", () => _sut.IsApplicable(null) as object);
		}

		[Fact]
		public void NotAllowNull_routeDataFunction_On_HasRouteData()
		{
			Assert.Throws<ArgumentNullException>("routeDataFunction", () => _sut.HasRouteData(null));
		}
	}
}