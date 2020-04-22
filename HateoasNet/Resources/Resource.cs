using System.Collections.Generic;

namespace HateoasNet.Resources
{
	/// <summary>
	/// Represents a formatted wrapper of requested data in addition to HATEOAS <see cref="Links"/>.
	/// </summary>
	public abstract class Resource
	{
		protected Resource(object data)
		{
			Data = data;
		}

		/// <summary>
		/// Requested original data
		/// </summary>
		public virtual object Data { get; }
		/// <summary>
		/// Additional <see cref="List{ResourceLink}" /> as HATEOAS data.
		/// </summary>
		public List<ResourceLink> Links { get; } = new List<ResourceLink>();
	}
}