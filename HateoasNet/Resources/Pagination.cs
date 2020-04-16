using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using HateoasNet.Abstractions;

namespace HateoasNet.Resources
{
	public class Pagination<T> : IPagination<T> where T : class
	{
		public Pagination(IEnumerable<T> data, long count, int pageSize, int page = 1)
		{
			Enumeration = data;
			Count = count;
			PageSize = pageSize;
			Page = page;
		}
		
		[JsonIgnore]
		public IEnumerable Enumeration { get; }
		public IEnumerable<T> Data => Enumeration as IEnumerable<T>;
		public long Count { get; }
		public int PageSize { get; }
		public int Page { get; }
		public int Pages => (int) (Count == 0 ? 1 : (Count + PageSize - 1) / PageSize);
	}
}