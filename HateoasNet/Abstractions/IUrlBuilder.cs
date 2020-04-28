using System.Collections.Generic;

namespace HateoasNet.Abstractions
{
	/// <summary>
	///   Represents an implementation which builds Urls of endpoint routes using Its names and route values.
	/// </summary>
	public interface IUrlBuilder
	{
		/// <summary>
		///   Builds an url <see cref="string" /> with the <paramref name="routeName" /> and <paramref name="routeDictionary" />.
		/// </summary>
		/// <param name="routeName">Name of desired route to discover the url.</param>
		/// <param name="routeDictionary">Route dictionary to look for parameters and query strings.</param>
		/// <returns>Generated Url <see cref="string" /> value.</returns>
		public string Build(string routeName, IDictionary<string, object> routeDictionary);
	}
}