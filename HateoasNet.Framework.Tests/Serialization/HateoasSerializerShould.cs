using System;
using HateoasNet.Abstractions;
using HateoasNet.Framework.Serialization;
using HateoasNet.Resources;
using Xunit;

namespace HateoasNet.Framework.Tests.Serialization
{
  public class HateoasSerializerShould
  {
    [Theory]
    [SerializationData]
    [Trait(nameof(IHateoasSerializer), nameof(IHateoasSerializer.SerializeResource))]
    public void ReturnsString_EqualTo_ExpectedOutput_FromCalling_SerializeResource<T>(T resource, string expectedOutput)
    where T : Resource
    {
      // arrange
      var sut = new HateoasSerializer();

      // act
      var actual = sut.SerializeResource(resource);

      // assert
      Assert.Equal(expectedOutput, actual);
    }

    [Fact]
    [Trait(nameof(IHateoasSerializer), nameof(IHateoasSerializer.SerializeResource))]
    [Trait(nameof(IHateoasSerializer), "Exceptions")]
    public void Throws_ArgumentNullException_FromCalling_SerializeResource()
    {
      Assert.Throws<ArgumentNullException>("resource", () => new HateoasSerializer().SerializeResource(null));
    }
  }
}
