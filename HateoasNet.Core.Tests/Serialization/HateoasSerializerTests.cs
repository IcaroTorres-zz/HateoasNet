using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using HateoasNet.Abstractions;
using HateoasNet.Core.Serialization;
using HateoasNet.Resources;
using Xunit;

namespace HateoasNet.Core.Tests.Serialization
{
	public class HateoasSerializerTests : IDisposable
	{
		/// <inheritdoc />
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

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

		[Theory]
		[AddConverterData]
		[Trait(nameof(IHateoasSerializer), nameof(HateoasSerializer.AddConverter))]
		public void AddConverter_WithValidJsonConverter_AddsNewConverter<T>(JsonConverter<T> converter)
		{
			// arrange
			var sut = new HateoasSerializer();
			sut.ResetConverters();

			// act
			sut.AddConverter(converter);

			// assert
			Assert.Contains(sut.Settings.Converters, x => x.GetType() == converter.GetType());
			Assert.Collection(sut.Settings.Converters, x => x.CanConvert(typeof(T)));
		}

		[Fact]
		[Trait(nameof(IHateoasSerializer), nameof(HateoasSerializer.ResetConverters))]
		public void ResetConverters_SetConverters_ToEmptyList()
		{
			// arrange
			var sut = new HateoasSerializer();
			var defaultConverters = new List<JsonConverter> {new GuidConverter(), new DateTimeConverter()};
			var initialConverters = new List<JsonConverter>(sut.Settings.Converters);

			// act
			sut.ResetConverters();

			// assert
			Assert.Contains(initialConverters, x => defaultConverters.Any(c => c.GetType() == x.GetType()));
			Assert.Collection(initialConverters, x => x.CanConvert(typeof(Guid)), x => x.CanConvert(typeof(DateTime)));
			Assert.Empty(sut.Settings.Converters);
		}

		[Fact]
		[Trait(nameof(IHateoasSerializer), nameof(IHateoasSerializer.SerializeResource))]
		[Trait(nameof(IHateoasSerializer), "Exceptions")]
		public void SerializeResource_WithResourceNull_Throws_ArgumentNullException()
		{
			// arrange
			var sut = new HateoasSerializer();
			const string parameterName = "resource";

			// act
			Action actual = () => sut.SerializeResource(null);

			Assert.Throws<ArgumentNullException>(parameterName, actual);
		}

		[Fact]
		[Trait(nameof(IHateoasSerializer), nameof(HateoasSerializer.AddConverter))]
		[Trait(nameof(IHateoasSerializer), "Exceptions")]
		public void Throws_ArgumentNullException_FromCalling_AddConverter()
		{
			// arrange
			var sut = new HateoasSerializer();
			const string parameterName = "converter";

			// act
			Action actual = () => sut.AddConverter<Guid>(null);

			Assert.Throws<ArgumentNullException>(parameterName, actual);
		}
	}
}
