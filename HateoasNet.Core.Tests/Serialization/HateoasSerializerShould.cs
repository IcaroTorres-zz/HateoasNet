using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using HateoasNet.Abstractions;
using HateoasNet.Core.Serialization;
using HateoasNet.Resources;
using Xunit;
using Xunit.Sdk;

namespace HateoasNet.Core.Tests.Serialization
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
		[Trait(nameof(IHateoasSerializer), nameof(HateoasSerializer.ResetConverters))]
		public void ResetConverters_ToEmptyList()
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

		[Theory]
		[AddConverterData]
		[Trait(nameof(IHateoasSerializer), nameof(HateoasSerializer.AddConverter))]
		public void AddConverter<T>(JsonConverter<T> converter)
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
		[Trait(nameof(IHateoasSerializer), nameof(IHateoasSerializer.SerializeResource))]
		[Trait(nameof(IHateoasSerializer), "Exceptions")]
		public void Throws_ArgumentNullException_FromCalling_SerializeResource()
		{
			Assert.Throws<ArgumentNullException>("resource", () => new HateoasSerializer().SerializeResource(null));
		}

		[Fact]
		[Trait(nameof(IHateoasSerializer), nameof(HateoasSerializer.AddConverter))]
		[Trait(nameof(IHateoasSerializer), "Exceptions")]
		public void Throws_ArgumentNullException_FromCalling_AddConverter()
		{
			Assert.Throws<ArgumentNullException>("converter", () => new HateoasSerializer().AddConverter<Guid>(null));
		}
	}

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
