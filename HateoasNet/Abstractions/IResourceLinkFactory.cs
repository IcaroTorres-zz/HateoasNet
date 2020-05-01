using System.Collections.Generic;
using HateoasNet.Resources;

namespace HateoasNet.Abstractions
{
	/// <summary>
	///   Represents a Factory for creation of <see cref="ResourceLink" /> instances.
	/// </summary>
	public interface IResourceLinkFactory
	{
		/// <summary>
		///   Creates a HATEOAS <see cref="ResourceLink" /> using <paramref name="rel" />
		///   with <paramref name="routeValues" /> to discover route Url and HTTP method.
		/// </summary>
		/// <param name="rel">
		///   An endpoint Route Name assigned on an action method attribute.
		///   <para>see also <seealso cref="IHateoasLink.RouteName" />.</para>
		/// </param>
		/// <param name="routeValues">
		///   The <see cref="IDictionary{TKey,TValue}" /> of <see cref="string" />, <see cref="object" /> holding route values.
		/// </param>
		/// <returns>The generated <see cref="ResourceLink" /> instance for HATEOAS output.</returns>
		ResourceLink Create(string rel, IDictionary<string, object> routeValues);
	}
}