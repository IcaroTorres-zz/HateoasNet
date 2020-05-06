using System.Collections.Generic;
using System.Reflection;
using HateoasNet.TestingObjects;
using Xunit.Sdk;

namespace HateoasNet.Tests.TestHelpers
{
	public class ConfigureDataAttribute : DataAttribute
	{
		public override IEnumerable<object[]> GetData(MethodInfo testMethod)
		{
			yield return new object[] {new Testee()};
			yield return new object[] {new NestedTestee()};
			yield return new object[]
			{
				new GenericTestee<object> {Nested = new Testee(), Collection = new List<object> {new Testee()}}
			};
		}
	}
}
