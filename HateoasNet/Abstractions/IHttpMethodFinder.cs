namespace HateoasNet.Abstractions
{
	/// <summary>
	/// Represents an implementation for discovering HTTP method of endpoint route by Its name.
	/// </summary>
	public interface IHttpMethodFinder
	{
		/// <summary>
		/// Find the <see cref="string"/> value of HTTP method of a route with given <paramref name="routeName"/>.
		/// </summary>
		/// <param name="routeName">The wanted endpoint route name to find.</param>
		/// <returns><see cref="string"/>value representing HTTP method.</returns>
		public string Find(string routeName);
	}
}