using System.Collections.Generic;

namespace HateoasNet.Abstractions
{
	/// <summary>
	/// Represents a HATEOAS resource to be formatted associate with <see cref="IHateoasLink"/> configurations.
	/// </summary>
	public interface IHateoasResource
	{
		/// <summary>
		/// Gets all <see cref="IHateoasLink" /> associated with this <see cref="IHateoasResource"/> instance.
		/// </summary>
		/// <returns><see cref="IEnumerable{IHateoasLink}"/> representing Its Links.</returns>
		IEnumerable<IHateoasLink> GetLinks();
	}

	/// <summary>
	/// Represents a HATEOAS resource <typeparamref name="T"/> to be formatted associate with <see cref="IHateoasLink{T}"/> configurations.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IHateoasResource<T> : IHateoasResource where T : class
	{
		/// <summary>
		/// Adds our replaces an <see cref="IHateoasLink{T}"/> configuration with <paramref name="routeName"/> to this
		/// <see cref="IHateoasResource{T}"/> instance.
		/// </summary>
		/// <param name="routeName">
		/// An endpoint Route Name assigned on an action method attribute.<para>see also <seealso cref="IHateoasLink{T}.RouteName"/>.</para>
		/// </param>
		/// <returns>A <see cref="IHateoasLink{T}"/> configuration generated with <paramref name="routeName"/>.</returns>
		IHateoasLink<T> HasLink(string routeName);
	}
}