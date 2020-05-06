using System.Collections;
using System.Collections.Generic;

namespace HateoasNet.Abstractions
{
    /// <summary>
    ///   Represents an <see cref="ICollection" /> paginated.
    /// </summary>
    public interface IPagination
    {
        long Count { get; }
        int PageSize { get; }
        int Page { get; }
        int Pages { get; }

        /// <summary>
        ///   Gets the paginated <see cref="ICollection" /> items as <see cref="IEnumerable" />.
        /// </summary>
        /// <returns></returns>
        IEnumerable GetItems();
    }

    /// <summary>
    ///   Represents an <see cref="ICollection{T}" /> paginated.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPagination<out T> : IPagination
    {
        /// <summary>
        ///   Gets paginated <see cref="ICollection{T}" /> items as <see cref="IEnumerable{T}" />.
        /// </summary>
        /// <returns>paginated <see cref="ICollection{T}" /> items as <see cref="IEnumerable{T}" />.</returns>
        new IEnumerable<T> GetItems();
    }
}