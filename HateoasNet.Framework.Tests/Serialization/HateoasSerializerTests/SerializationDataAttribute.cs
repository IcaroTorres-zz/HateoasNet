using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using HateoasNet.Resources;
using HateoasNet.TestingObjects;
using Xunit.Sdk;

namespace HateoasNet.Framework.Tests.Serialization.HateoasSerializerTests
{
  public class SerializationData : DataAttribute
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
      var testees = new List<Testee> { testee, nestedTestee, genericTestee };

      // resources
      var resources = testees.Select(x =>
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
      var expectedSingleString = File.ReadAllText(
        Path.Combine("Serialization", "HateoasSerializerTests", "SingleResource.json"));
      var expectedEnumerableString = File.ReadAllText(
        Path.Combine("Serialization", "HateoasSerializerTests", "EnumerableResource.json"));
      var expectedPaginationString = File.ReadAllText(
        Path.Combine("Serialization", "HateoasSerializerTests", "PaginationResource.json"));

      yield return new object[] { resources.First(), expectedSingleString };
      yield return new object[] { enumerableResource, expectedEnumerableString };
      yield return new object[] { paginationResource, expectedPaginationString };
    }
  }
}