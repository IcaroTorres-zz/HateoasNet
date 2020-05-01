using System.Collections.Generic;
using System.Reflection;
using HateoasNet.Resources;
using HateoasNet.TestingObjects;
using Xunit.Sdk;

namespace HateoasNet.Tests.TestHelpers
{
	public class CreateResourceDataAttribute : DataAttribute
	{
		/// <inheritdoc />
		public override IEnumerable<object[]> GetData(MethodInfo testMethod)
		{
			// dummies
			var testee = new Testee();
			var nestedTestee = new NestedTestee();
			var genericTesteeObject = new GenericTestee<object>();
			var genericTesteeTestee = new GenericTestee<Testee>();
			var genericTesteeNestedTestee = new GenericTestee<NestedTestee>();
			var listTestee = new List<Testee>
			{
				testee, nestedTestee, genericTesteeObject, genericTesteeTestee, genericTesteeNestedTestee
			};
			var paginationTestee = new Pagination<Testee>(listTestee, 5, 2, 2);

			yield return new object[] {testee};
			yield return new object[] {nestedTestee};
			yield return new object[] {genericTesteeObject};
			yield return new object[] {genericTesteeTestee};
			yield return new object[] {genericTesteeNestedTestee};
			yield return new object[] {listTestee};
			yield return new object[] {paginationTestee};
		}
	}
}
