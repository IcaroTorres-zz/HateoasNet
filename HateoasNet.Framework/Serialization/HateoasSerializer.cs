using HateoasNet.Abstractions;
using HateoasNet.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HateoasNet.Framework.Serialization
{
	public class HateoasSerializer : IHateoasSerializer
	{
		public string SerializeResource(Resource resource)
		{
			var jsonSerializerSettings = new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore,
				ContractResolver = new CamelCasePropertyNamesContractResolver()
			};
			return JsonConvert.SerializeObject(resource, jsonSerializerSettings);
		}
	}
}