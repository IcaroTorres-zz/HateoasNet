using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HateoasNet.Converters
{
	public class GuidConverter : JsonConverter<Guid>
	{
		public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return Guid.Parse(reader.GetString());
		}

		public override void Write(Utf8JsonWriter writer, Guid guidValue, JsonSerializerOptions options)
		{
			writer.WriteStringValue(guidValue.ToString());
		}
	}
}