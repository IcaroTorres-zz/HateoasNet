namespace HateoasNet.Resources
{
	/// <summary>
	/// Represents a HATEOAS link witch is an item of <see cref="Resource.Links"/>.
	/// </summary>
	public class ResourceLink
	{
		public ResourceLink(string rel, string href, string method)
		{
			Rel = rel;
			Href = href;
			Method = method;
		}

		/// <summary>
		/// The Url link address to access some available action of the <see cref="Resource"/> containing this <see cref="ResourceLink"/>.
		/// </summary>
		/// <value>Url <see cref="string"/> value.</value>
		public string Href { get; }
		
		/// <summary>
		/// The name assigned to the action available for <see cref="Resource"/> containing this <see cref="ResourceLink"/>.
		/// </summary>
		/// <value>The <see cref="string"/> value of assigned <see cref="Rel"/>.</value>
		public string Rel { get; }
		
		/// <summary>
		/// The HTTP method used to access the action available for <see cref="Resource"/> containing this <see cref="ResourceLink"/>.
		/// </summary>
		/// <value>The <see cref="string"/> value of HTTP method.</value>
		public string Method { get; }
	}
}