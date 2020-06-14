using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace HateoasNet.Tests.TestHelpers
{
    public class WhenDataAttribute : DataAttribute
    {
        /// <inheritdoc />
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var testee = new HateoasSample();

            yield return new object[] { testee, new Func<HateoasSample, bool>(x => !string.IsNullOrWhiteSpace(x.Email)) };
            yield return new object[] { testee, new Func<HateoasSample, bool>(x => !string.IsNullOrWhiteSpace(x.DocumentNumber)) };
            yield return new object[] { testee, new Func<HateoasSample, bool>(x => !string.IsNullOrWhiteSpace(x.FileName)) };
            yield return new object[] { testee, new Func<HateoasSample, bool>(x => x.ForeignKeyId != Guid.Empty) };
        }
    }
}
