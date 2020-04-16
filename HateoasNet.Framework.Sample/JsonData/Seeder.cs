using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace HateoasNet.Framework.Sample.JsonData
{
	public class Seeder
	{
		internal List<T> Seed<T>() where T : class
		{
			using var stream = new StreamReader($"JsonData\\{typeof(T).Name.ToLower()}s.json");
			return JsonConvert.DeserializeObject<List<T>>(stream.ReadToEnd());
		}
	}
}