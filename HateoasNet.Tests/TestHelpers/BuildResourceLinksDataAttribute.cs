using HateoasNet.Resources;
using HateoasNet.TestingObjects;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace HateoasNet.Tests.TestHelpers
{
    public class BuildResourceLinksDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var testee = new Testee();
            var listTestee = new List<Testee> { testee };
            var paginationTestee = new Pagination<Testee>(listTestee, 1, 10);

            var singleResources = listTestee.Select(x => new SingleResource(x)).ToList();
            var enumerableResource = new EnumerableResource(singleResources);
            var paginationResources = new PaginationResource(singleResources, 1, 10, 1);

            yield return new object[] { new SingleResource(testee), testee, typeof(Testee) };                //,(Testee) null};
            yield return new object[] { enumerableResource, listTestee, typeof(List<Testee>) };              //, testee};
            yield return new object[] { paginationResources, paginationTestee, typeof(Pagination<Testee>) }; //, testee};
        }
    }
}
