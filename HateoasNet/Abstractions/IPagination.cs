using System.Collections;
using System.Collections.Generic;

namespace HateoasNet.Abstractions
{
	public interface IPagination
	{
		long Count { get; }
		int PageSize { get; }
		int Page { get; }
		int Pages { get; }
		IEnumerable GetEnumeration();
	}

	public interface IPagination<out T> : IPagination
	{
		IEnumerable<T> Data { get; }
	}
}