using System.Collections;
using System.Collections.Generic;

namespace HateoasNet.Abstractions
{
	public interface IPagination
	{
		IEnumerable Enumeration { get; }
		long Count { get; }
		int PageSize { get; }
		int Page { get; }
		int Pages { get; }
	}

	public interface IPagination<out T> : IPagination
	{
		IEnumerable<T> Data { get; }
	}
}