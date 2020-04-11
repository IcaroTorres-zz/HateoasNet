﻿using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using HateoasNet.Converters;

namespace Sample.JsonData
{
	public static class Seeder
	{
		internal static List<T> Seed<T>() where T : class
		{
			var serializerOptions = new JsonSerializerOptions
			{
				IgnoreNullValues = true,
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			};
			serializerOptions.Converters.Add(new GuidConverter());
			serializerOptions.Converters.Add(new DateTimeConverter());
			
			using var stream = new StreamReader($"JsonData\\{typeof(T).Name.ToLower()}s.json");
			return JsonSerializer.Deserialize<List<T>>(stream.ReadToEnd(), serializerOptions);
		}
	}
}