using System.Collections.Generic;

namespace HateoasNet.Resources
{
	public class Pagination<T> where T : class
	{
		public Pagination(IEnumerable<T> data, long count, int pageSize, int page = 1)
		{
			Data = data;
			Count = count;
			PageSize = pageSize;
			Page = page;
		}

		public IEnumerable<T> Data { get; }
		public long Count { get; }
		public int PageSize { get; }
		public int Page { get; }
		public int Pages => (int) (Count == 0 ? 1 : (Count + PageSize - 1) / PageSize);
	}
}