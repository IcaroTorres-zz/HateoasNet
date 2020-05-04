using HateoasNet.Resources;
using System;
using System.Collections;

namespace HateoasNet.Abstractions
{
    /// <summary>
    ///   Represents a Factory for creation of formatted <see cref="Resource" /> output instances.
    /// </summary>
    public interface IResourceFactory
    {
        /// <summary>
        ///   Creates a formatted <see cref="SingleResource" /> using <see cref="IHateoasResource{T}" /> configuration
        ///   of <paramref name="type" /> with <paramref name="source" /> as original value.
        /// </summary>
        /// <param name="source">Original value to wrap in a <see cref="SingleResource" />.</param>
        /// <param name="type">type parameter of <see cref="IHateoasResource{T}" />  configuration.</param>
        /// <returns>A formatted <see cref="SingleResource" /> instance.</returns>
        SingleResource Create(object source, Type type);

        /// <summary>
        ///   Creates a formatted <see cref="EnumerableResource" /> using <see cref="IHateoasResource{T}" />
        ///   configuration
        ///   of <paramref name="type" /> with <paramref name="source" /> as original value.
        /// </summary>
        /// <param name="source">Original value to wrap in a <see cref="SingleResource" /></param>
        /// <param name="type">type parameter of <see cref="IHateoasResource{T}" /> configuration.</param>
        /// <returns>A formatted <see cref="EnumerableResource" /> instance.</returns>
        EnumerableResource Create(IEnumerable source, Type type);

        /// <summary>
        ///   Creates a formatted <see cref="PaginationResource" /> using <see cref="IHateoasResource{T}" />
        ///   configuration of <paramref name="type" /> with <paramref name="source" /> as original value.
        /// </summary>
        /// <param name="source">Original value to wrap in a <see cref="SingleResource" /></param>
        /// <param name="type">type parameter of <see cref="IHateoasResource{T}" /> configuration.</param>
        /// <returns>A formatted <see cref="PaginationResource" /> instance.</returns>
        PaginationResource Create(IPagination source, Type type);
    }
}
