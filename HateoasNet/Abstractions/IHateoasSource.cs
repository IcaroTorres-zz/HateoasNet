using System.Collections.Generic;

namespace HateoasNet.Abstractions
{
    /// <summary>
    ///   Represents a HATEOAS Source to generate <see cref="IEnumerable{T}"/> of <see cref="HateoasLink"/> 
    ///   items associate with <see cref="IHateoasLinkBuilder" />.
    /// </summary>
    public interface IHateoasSource
    {
        /// <summary>
        ///   Gets all <see cref="IHateoasLinkBuilder" /> associated with this <see cref="IHateoasSource" /> instance.
        /// </summary>
        /// <returns><see cref="IEnumerable{IHateoasLink}" /> representing Its Links.</returns>
        IEnumerable<IHateoasLinkBuilder> GetLinkBuilders();
    }

    /// <summary>
    ///   Represents a HATEOAS resource <typeparamref name="T" /> to be formatted associate with <see cref="IHateoasLinkBuilder{T}" />
    ///   configurations.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHateoasSource<T> : IHateoasSource where T : class
    {
        /// <summary>
        ///   Adds our replaces an <see cref="IHateoasLinkBuilder{T}" /> configuration with <paramref name="routeName" /> to this
        ///   <see cref="IHateoasSource{T}" /> instance.
        /// </summary>
        /// <param name="routeName">
        ///   An endpoint Route Name assigned on an action method attribute.
        ///   <para>see also <seealso cref="IHateoasLinkBuilder.RouteName" />.</para>
        /// </param>        
        /// <returns>A <see cref="IHateoasLinkBuilder{T}" /> configuration generated with <paramref name="routeName" />.</returns>
        IHateoasLinkBuilder<T> AddLink(string routeName);
    }
}
