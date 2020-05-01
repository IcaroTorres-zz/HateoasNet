using System.Collections.Generic;
using System.Reflection;
using HateoasNet.TestingObjects;
using Xunit.Sdk;

namespace HateoasNet.Tests.TestHelpers
{
	public class ConfigurationDataAttribute : DataAttribute
	{
		public override IEnumerable<object[]> GetData(MethodInfo testMethod)
		{
			yield return new object[] {new TesteeResourceConfiguration(), new Testee()};
			yield return new object[] {new NestedTesteeResourceConfiguration(), new NestedTestee()};
			yield return new object[]
			{
				new GenericTesteeResourceConfiguration(), new GenericTestee<object>
				{
					Nested = new Testee(), Collection = new List<object> {new Testee()}
				}
			};
		}
	}
}
