using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HateoasNet.Core.Serialization
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        private readonly string _dateTimeReadFormat;
        private readonly CultureInfo _cultureInfo;

        public DateTimeConverter(string dateTimeReadFormat = null, CultureInfo cultureInfo = null)
        {
            _dateTimeReadFormat = string.IsNullOrWhiteSpace(dateTimeReadFormat) ? "dd/MM/yyyy" : dateTimeReadFormat;
            _cultureInfo = cultureInfo ?? CultureInfo.InvariantCulture;
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.ParseExact(reader.GetString(), _dateTimeReadFormat, _cultureInfo);
        }

        public override void Write(Utf8JsonWriter writer, DateTime date, JsonSerializerOptions options)
        {
            writer.WriteStringValue(date.ToUniversalTime());
        }
    }
}