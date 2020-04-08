using System.Collections.Generic;

namespace HateoasNet.Resources
{
	public class Pagination<T> where T : class
	{
		public Pagination(IEnumerable<T> data, long count, int pageSize, int page = 1)
		{
			Data = data;
			Page = page;
			PageSize = pageSize;
			Count = count;
		}

		public IEnumerable<T> Data { get; }
		public int Page { get; }
		public int PageSize { get; }
		public long Count { get; }
		public long Pages => Count == 0 ? 1 : (Count + PageSize - 1) / PageSize;
	}
}