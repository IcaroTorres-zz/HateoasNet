using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HateoasNet.Abstractions;

namespace HateoasNet.Resources
{
	/// <inheritdoc cref="IPagination{T}" />
	public class Pagination<T> : IPagination<T> where T : class
	{
		public Pagination(IEnumerable<T> data, long count, int pageSize, int page = 1)
		{
			Data = data.ToArray();
			Count = count;
			PageSize = pageSize;
			Page = page;
		}

		public IEnumerable GetEnumeration()
		{
			return Data;
		}

		public IEnumerable<T> Data { get; }
		public long Count { get; }
		public int PageSize { get; }
		public int Page { get; }
		public int Pages => (int) (Count == 0 ? 1 : (Count + PageSize - 1) / PageSize);
	}
}