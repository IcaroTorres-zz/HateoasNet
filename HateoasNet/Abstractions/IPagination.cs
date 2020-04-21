using System.Collections;
using System.Collections.Generic;

namespace HateoasNet.Abstractions
{
	/// <summary>
	/// Represents an <see cref="ICollection"/> paginated.
	/// </summary>
	public interface IPagination
	{
		long Count { get; }
		int PageSize { get; }
		int Page { get; }
		int Pages { get; }
		
		/// <summary>
		/// Gets the paginated <see cref="ICollection"/> items as <see cref="IEnumerable" />.
		/// </summary>
		/// <returns></returns>
		IEnumerable GetEnumeration();
	}

	/// <summary>
	/// Represents an <see cref="ICollection{T}"/> paginated.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IPagination<out T> : IPagination
	{
		/// <summary>
		/// The paginated <see cref="ICollection{T}"/> items as <see cref="IEnumerable{T}"/>.
		/// </summary>
		IEnumerable<T> Data { get; }
	}
}