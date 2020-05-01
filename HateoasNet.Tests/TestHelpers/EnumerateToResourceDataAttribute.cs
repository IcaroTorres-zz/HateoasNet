using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HateoasNet.TestingObjects;
using Xunit.Sdk;

namespace HateoasNet.Tests.TestHelpers
{
	public class EnumerateToResourceDataAttribute : DataAttribute
	{
		/// <inheritdoc />
		public override IEnumerable<object[]> GetData(MethodInfo testMethod)
		{
			yield return new object[]
			{
				new List<Testee>()
				{
					new Testee(), new NestedTestee(), new GenericTestee<Testee>(),
					new GenericTestee<NestedTestee>(), new GenericTestee<object>()
				}
			};
			yield return new object[] {Enumerable.Range(1, 5).Select(_ => new NestedTestee()).ToList()};
			yield return new object[] {Enumerable.Range(1, 5).Select(_ => new GenericTestee<Testee>()).ToList()};
			yield return new object[] {Enumerable.Range(1, 5).Select(_ => new GenericTestee<NestedTestee>()).ToList()};
			yield return new object[] {Enumerable.Range(1, 5).Select(_ => new GenericTestee<object>()).ToList()};
		}
	}
}
