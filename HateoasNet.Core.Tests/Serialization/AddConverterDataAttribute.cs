using System.Collections.Generic;
using System.Reflection;
using HateoasNet.Core.Serialization;
using Xunit.Sdk;

namespace HateoasNet.Core.Tests.Serialization
{
	internal class AddConverterDataAttribute : DataAttribute
	{
		/// <inheritdoc />
		public override IEnumerable<object[]> GetData(MethodInfo testMethod)
		{
			yield return new object[] {new GuidConverter()};
			yield return new object[] {new DateTimeConverter()};
		}
	}
}