using HateoasNet.Core.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HateoasNet.Core.Sample.JsonData
{
    public class Seeder
    {
        internal List<T> Seed<T>() where T : class
        {
            var serializerOptions = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            serializerOptions.Converters.Add(new GuidConverter());
            serializerOptions.Converters.Add(new DateTimeConverter());

            using var stream = new StreamReader($"JsonData\\{typeof(T).Name.ToLower()}s.json");
            return JsonSerializer.Deserialize<List<T>>(stream.ReadToEnd(), serializerOptions);
        }
    }
}