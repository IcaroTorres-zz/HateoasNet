using HateoasNet.Resources;
using HateoasNet.TestingObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace HateoasNet.Core.Tests.Formatting
{
    public class FormattingDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            // Guid and DateTime values shared for serialization test
            var sharedGuid = Guid.Parse("96685bc4-dcb7-4b22-90cc-ca83baff8186");
            var sharedDateTime = DateTime.Parse("Fri, 1 May 2020 00:00:00Z");

            // dummies
            var emptyLink = new ResourceLink("", "", "");
            var testee = new Testee { Id = sharedGuid, CreatedDate = sharedDateTime };
            var nestedTestee = new NestedTestee
            {
                Id = sharedGuid,
                CreatedDate = sharedDateTime,
                Nested = testee,
                Collection = new List<Testee> { testee }
            };
            var genericTestee = new GenericTestee<Testee>
            {
                Nested = nestedTestee,
                Id = sharedGuid,
                CreatedDate = sharedDateTime,
                Collection = new List<Testee> { testee, nestedTestee }
            };
            var testeeList = new List<Testee> { testee, nestedTestee, genericTestee };
            var testeePagination = new Pagination<Testee>(testeeList, testeeList.Count, 5);

            // resources
            var resources = testeeList.Select(x =>
            {
                var resource = new SingleResource(x);
                resource.Links.Add(emptyLink);
                return resource;
            }).ToList();

            var enumerableResource = new EnumerableResource(resources);
            enumerableResource.Links.Add(emptyLink);
            var paginationResource = new PaginationResource(resources, resources.Count(), 5, 1);
            paginationResource.Links.Add(emptyLink);

            // expectedOutputs
            var expectedSingleString = File.ReadAllText(Path.Combine("Serialization", "SingleResource.json"));
            var expectedEnumerableString = File.ReadAllText(Path.Combine("Serialization", "EnumerableResource.json"));
            var expectedPaginationString = File.ReadAllText(Path.Combine("Serialization", "PaginationResource.json"));

            yield return new object[] { testee, resources.First(), expectedSingleString };
            yield return new object[] { testeeList, enumerableResource, expectedEnumerableString };
            yield return new object[] { testeePagination, paginationResource, expectedPaginationString };
        }
    }
}
