using System.Collections.Generic;

namespace HateoasNet.Resources
{
	public abstract class Resource
	{
		protected Resource(object data)
		{
			Data = data;
		}

		public virtual object Data { get; }
		public List<ResourceLink> Links { get; } = new List<ResourceLink>();
	}
}