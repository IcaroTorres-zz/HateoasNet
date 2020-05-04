using System;
using HateoasNet.Abstractions;
using HateoasNet.Framework.Serialization;
using HateoasNet.Resources;
using Xunit;

namespace HateoasNet.Framework.Tests.Serialization
{
	public class HateoasSerializerTests : IDisposable
	{
		[Theory]
		[SerializationData]
		[Trait(nameof(IHateoasSerializer), nameof(IHateoasSerializer.SerializeResource))]
		public void SerializeResource_WithValidResource_ReturnsExpectedString(Resource resource, string expectedOutput)
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
		public void SerializeResource_WithNull_Throws_ArgumentNullException()
		{
			// arrange
			var sut = new HateoasSerializer();
			const string parameterName = "resource";

			// act
			Action actual = () => sut.SerializeResource(null);

			Assert.Throws<ArgumentNullException>(parameterName, actual);
		}

		/// <inheritdoc />
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}
