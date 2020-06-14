using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.IO;

namespace HateoasNet.Core.Sample.JsonData
{
	public class Seeder
	{
		internal List<T> Seed<T>() where T : class
		{
			var serializerSettings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				NullValueHandling = NullValueHandling.Ignore,
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			};

			using var stream = new StreamReader($"JsonData\\{typeof(T).Name.ToLower()}s.json");
			return JsonConvert.DeserializeObject<List<T>>(stream.ReadToEnd(), serializerSettings);
		}
	}
}
