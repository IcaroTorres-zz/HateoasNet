namespace HateoasNet.Abstractions
{
	/// <summary>
	/// Interface for specific platform implementations of HttpMethod discovering of routes by name.
	/// </summary>
	public interface IHttpMethodFinder
	{
		/// <summary>
		/// Find the HttpMethod string value for a route with given name.
		/// </summary>
		/// <param name="routeName">Route name to find.</param>
		/// <returns>The string value representing HttpMethod.</returns>
		public string Find(string routeName);
	}
}