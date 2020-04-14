using System.Text.Json;
using HateoasNet.Abstractions;
using HateoasNet.Converters;
using HateoasNet.Resources;

namespace HateoasNet.Formatting
{
	public class HateoasSerializer : IHateoasSerializer
	{
		public string SerializeResource(Resource resource)
		{
			var serializerOptions = new JsonSerializerOptions
			{
				IgnoreNullValues = true,
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};
			serializerOptions.Converters.Add(new GuidConverter());
			serializerOptions.Converters.Add(new DateTimeConverter());
			return JsonSerializer.Serialize(resource, serializerOptions);
		}
	}
}