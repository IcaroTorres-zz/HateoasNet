using System;
using System.Collections.Generic;

namespace HateoasNet.Resources
{
    /// <summary>
    ///   Represents a formatted wrapper of requested data in addition to HATEOAS <see cref="Links" />.
    /// </summary>
    [Serializable]
	public abstract class Resource
	{
		protected Resource(object data)
		{
			Data = data;
		}

        /// <summary>
        ///   Requested original data
        /// </summary>
        public virtual object Data { get; }

        /// <summary>
        ///   Additional <see cref="List{T}" /> as HATEOAS data.
        /// </summary>
        public virtual List<ResourceLink> Links { get; } = new List<ResourceLink>();
	}
}
