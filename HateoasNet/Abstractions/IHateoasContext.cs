using System;
using System.Collections.Generic;
using System.Reflection;

namespace HateoasNet.Abstractions
{
    /// <summary>
    ///   Represents the entry point for configuring HATEOAS integration.
    /// </summary>
    public interface IHateoasContext
	{
        /// <summary>
        ///   Checks if <see cref="IHateoasContext" /> has a <see cref="IHateoasResource{T}" />
        ///   for T being <paramref name="type" />.
        /// </summary>
        /// <param name="type">Type to look for <see cref="IHateoasResource{T}" /> associated.</param>
        /// <returns>
        ///   <seealso langword="bool" /> if exists a <see cref="IHateoasResource{T}" />
        ///   for T being <paramref name="type" />.
        /// </returns>
        bool HasResource(Type type);

        /// <summary>
        ///   Gets all applicable <see cref="IHateoasLink" /> from a <see cref="IHateoasResource{T}" />
        ///   for T being <paramref name="type" /> using <paramref name="value" /> to evaluate the applicability.
        /// </summary>
        /// <param name="type">Type to look for <see cref="IHateoasResource{T}" /> associated.</param>
        /// <param name="value">An object used to evaluate the applicability.</param>
        /// <returns><see cref="IEnumerable{IHateoasLink}" /> with all applicable <see cref="IHateoasLink" />.</returns>
        IEnumerable<IHateoasLink> GetApplicableLinks(Type type, object value);

        /// <summary>
        ///   Adds or continues an <see cref="IHateoasResource{T}" /> configuration of
        ///   <typeparamref name="T" /> using an <see cref="Action{T}" /> of <see cref="IHateoasResource{T}" /> action.
        /// </summary>
        /// <param name="resource">Action enabling configuration over the instance of IHateoasResource{T}.</param>
        /// <typeparam name="T">Target class for resource configuration.</typeparam>
        /// <returns>Current <see cref="IHateoasContext" /> instance.</returns>
        IHateoasContext Configure<T>(Action<IHateoasResource<T>> resource) where T : class;

        /// <summary>
        ///   Adds or continues an <see cref="IHateoasResource{T}" /> configuration of <typeparamref name="T" /> using
        ///   a <see cref="IHateoasResourceConfiguration{T}" /> instance.
        /// </summary>
        /// <param name="configuration">
        ///   A <see cref="IHateoasResourceConfiguration{T}" /> instance implementing the configuration of
        ///   <typeparamref name="T" /> in separated class.
        /// </param>
        /// <typeparam name="T">Target class for resource configuration.</typeparam>
        /// <returns>Current <see cref="IHateoasContext" /> instance.</returns>
        IHateoasContext ApplyConfiguration<T>(IHateoasResourceConfiguration<T> configuration) where T : class;

        /// <summary>
        ///   Applies all configurations of HATEOAS resources found in classes implementing
        ///   <see cref="IHateoasResourceConfiguration{T}" />
        ///   on given <paramref name="assembly" /> through their <see cref="IHateoasResourceConfiguration{T}.Configure" /> method.
        /// </summary>
        /// <param name="assembly">Target assembly containing classes implementing <see cref="IHateoasResourceConfiguration{T}" />.</param>
        /// <returns>Current <see cref="IHateoasContext" /> instance.</returns>
        IHateoasContext ConfigureFromAssembly(Assembly assembly);
	}
}
