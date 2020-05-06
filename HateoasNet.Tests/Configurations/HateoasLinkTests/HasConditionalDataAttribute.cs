using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HateoasNet.TestingObjects;
using Xunit.Sdk;

namespace HateoasNet.Tests.Configurations.HateoasLinkTests
{
	public class HasConditionalDataAttribute : DataAttribute
	{
		/// <inheritdoc />
		public override IEnumerable<object[]> GetData(MethodInfo testMethod)
		{
			var testee = new Testee();
			var nestedTestee = new NestedTestee();
			var genericTestee = new GenericTestee<Testee> {Nested = testee, Collection = new List<Testee> {testee}};

			yield return new object[] {testee, new Func<Testee, bool>(x => x.BoolValue)};
			yield return new object[] {testee, new Func<Testee, bool>(x => x.DecimalValue > 2000m)};
			yield return new object[] {testee, new Func<Testee, bool>(x => !string.IsNullOrWhiteSpace(x.StringValue))};
			yield return new object[] {testee, new Func<Testee, bool>(x => x.DecimalValue == new decimal(x.FloatValue))};

			yield return new object[] {nestedTestee, new Func<NestedTestee, bool>(x => x.Nested.BoolValue)};
			yield return new object[] {nestedTestee, new Func<NestedTestee, bool>(x => x.Nested.DecimalValue > 2000m)};
			yield return new object[]
			{
				nestedTestee, new Func<NestedTestee, bool>(x => !string.IsNullOrWhiteSpace(x.Nested.StringValue))
			};
			yield return new object[]
			{
				nestedTestee, new Func<NestedTestee, bool>(x => x.DecimalValue == new decimal(x.Nested.FloatValue))
			};

			yield return new object[]
			{
				genericTestee,
				new Func<GenericTestee<Testee>, bool>(x => x.Collection.First().BoolValue)
			};
			yield return new object[]
			{
				genericTestee, new Func<GenericTestee<Testee>, bool>(x => x.Collection.First().DecimalValue > 2000m)
			};
			yield return new object[]
			{
				genericTestee,
				new Func<GenericTestee<Testee>, bool>(x => !string.IsNullOrWhiteSpace(x.Collection.First().StringValue))
			};
			yield return new object[]
			{
				genericTestee,
				new Func<GenericTestee<Testee>, bool>(x => x.Collection.First().DecimalValue == new decimal(x.FloatValue))
			};
		}
	}
}
