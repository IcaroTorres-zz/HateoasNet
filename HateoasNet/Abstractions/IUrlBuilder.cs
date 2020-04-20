using System.Collections.Generic;

namespace HateoasNet.Abstractions
{
	/// <summary>
	/// Custom interface for creating urls for routes
	/// </summary>
	public interface IUrlBuilder
	{
		/// <summary>
		/// Build an url string for a route with given name and values from route data.
		/// </summary>
		/// <param name="routeName">Name of desired route to discover the url.</param>
		/// <param name="routeData">Route data object to look for parameters and query strings.</param>
		/// <returns>Generated Url string.</returns>
		public string Build(string routeName, object routeData);

		/// <summary>
		/// Build an url string for a route with given name and values from route data.
		/// </summary>
		/// <param name="routeName">Name of desired route to discover the url.</param>
		/// <param name="routeDictionary">Route dictionary to look for parameters and query strings.</param>
		/// <returns>Generated Url string.</returns>
		public string Build(string routeName, IDictionary<string, object> routeDictionary);
	}
}