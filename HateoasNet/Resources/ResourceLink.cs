using System;

namespace HateoasNet.Resources
{
    /// <summary>
    ///   Represents an item of <see cref="Resource.Links" /> holding a <see cref="Rel" />,
    ///   <see cref="Href" /> and <see cref="Method" />.
    /// </summary>
    [Serializable]
	public class ResourceLink
	{
		public ResourceLink(string rel, string href, string method)
		{
			Rel = rel;
			Href = href;
			Method = method;
		}

        /// <summary>
        ///   The Url to access some available action of the <see cref="Resource" /> containing this
        ///   <see cref="ResourceLink" />.
        /// </summary>
        /// <value>Url <see langword="string" /> value.</value>
        public virtual string Href { get; } = string.Empty;

        /// <summary>
        ///   The name assigned to the action available for <see cref="Resource" /> containing this <see cref="ResourceLink" />.
        /// </summary>
        /// <value>The <see langword="string" /> value of assigned <see cref="Rel" />.</value>
        public virtual string Rel { get; } = string.Empty;

        /// <summary>
        ///   The HTTP method used to access the action available for <see cref="Resource" /> containing this
        ///   <see cref="ResourceLink" />.
        /// </summary>
        /// <value>The <see langword="string" /> value of HTTP method.</value>
        public virtual string Method { get; } = string.Empty;
	}
}
