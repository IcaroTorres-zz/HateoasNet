using System;
using System.Collections.Generic;
using HateoasNet.Resources;

namespace HateoasNet.Abstractions
{
    /// <summary>
    ///   Represents a configuration of HATEOAS link displayed to response output.
    /// </summary>
    public interface IHateoasLink
	{
        /// <summary>
        ///   An endpoint Route Name assigned on an action method attribute.
        /// </summary>
        /// <value>
        ///   The <see cref="System.string" /> value for <see cref="ResourceLink.Rel" /> property of generated HATEOAS link.
        /// </value>
        string RouteName { get; }

        /// <summary>
        ///   Gets a <see cref="IDictionary{TKey, TValue}" /> of <see langword="string" />,
        ///   <see langword="object" /> representing the route values.
        /// </summary>
        /// <param name="resourceData">
        ///   An object to be used as parameter of <see cref="IHateoasLink{T}.RouteDictionaryFunction" />
        ///   to generate de dictionary.
        /// </param>
        /// <returns>
        ///   Generated route values dictionary <see cref="IDictionary{TKey, TValue}" /> of <see langword="string" />,
        ///   <see langword="object" />.
        /// </returns>
        IDictionary<string, object> GetRouteDictionary(object resourceData);

        /// <summary>
        ///   Checks if this <see cref="IHateoasLink" /> instance is applicable to output of <paramref name="resourceData" /> .
        /// </summary>
        /// <param name="resourceData">
        ///   /// An object to be used as parameter of <see cref="IHateoasLink{T}.Predicate" />
        ///   to evaluate predicate.
        /// </param>
        /// <returns>
        ///   <seealso cref="System.bool" /> if this <see cref="IHateoasLink" /> for <paramref name="resourceData" />
        ///   passes <see cref="IHateoasLink{T}.Predicate" /> for applicability.
        /// </returns>
        bool IsApplicable(object resourceData);
	}

    /// <summary>
    ///   Represents a <typeparamref name="T" /> configuration of HATEOAS link displayed to response output.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHateoasLink<T> : IHateoasLink //where T : class
	{
        /// <summary>
        ///   A framework function to generate <see cref="IDictionary{TKey, TValue}" /> of <see langword="string" />,
        ///   <see langword="object" /> representing the route values.
        /// </summary>
        Func<T, IDictionary<string, object>> RouteDictionaryFunction { get; }

        /// <summary>
        ///   An user defined predicate function to filter applicability of <see cref="IHateoasLink{T}" />.
        /// </summary>
        Func<T, bool> Predicate { get; }

        /// <summary>
        ///   Receives an user defined function returning anonymous <see langword="object" /> for ease usage, converting it to
        ///   <see cref="RouteDictionaryFunction" /> internally.
        /// </summary>
        /// <param name="routeDataFunction">Function returning anonymous <see langword="object" /> for ease usage.</param>
        /// <returns>Current <see cref="IHateoasLink{T}" /> instance.</returns>
        IHateoasLink<T> HasRouteData(Func<T, object> routeDataFunction);

        /// <summary>
        ///   Receives an user defined predicate function to filter applicability of <see cref="IHateoasLink{T}" />
        /// </summary>
        /// <param name="predicate">Predicate function to filter applicability of <see cref="IHateoasLink{T}" />.</param>
        /// <returns>Current <see cref="IHateoasLink{T}" /> instance.</returns>
        IHateoasLink<T> HasConditional(Func<T, bool> predicate);
	}
}
