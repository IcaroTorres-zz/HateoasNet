using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HateoasNet.Resources
{
	public abstract class Resource
	{
		protected Resource(object data)
		{
			Data = data;
		}

		[JsonPropertyName("data")] public virtual object Data { get; }
		[JsonPropertyName("links")] public virtual List<ResourceLink> Links { get; } = new List<ResourceLink>();
	}
}