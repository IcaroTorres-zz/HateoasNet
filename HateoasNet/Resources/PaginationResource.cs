using System.Collections.Generic;
using System.Linq;
using HateoasNet.Abstractions;

namespace HateoasNet.Resources
{
	public class PaginationResource<T> : Resource where T : Resource
	{
		public PaginationResource(IPagination<T> values) : base(values.Data)
		{
			DataList = values.Data.ToList();
			Count = values.Count;
			PageSize = values.PageSize;
			Page = values.Page;
			Pages = values.Pages;
		}

		private List<T> DataList { get; }
		public override object Data => DataList;
		public int InPage => DataList.Count;
		public int Page { get; }
		public int PageSize { get; }
		public int Pages { get; }
		public long Count { get; }
	}
}