using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace HateoasNet.Resources
{
	public class PaginationResource<T> : Resource where T : Resource
	{
		public PaginationResource(Pagination<T> values) : base(values.Data)
		{
			DataList = values.Data.ToList();
			Count = values.Count;
			PageSize = values.PageSize;
			Page = values.Page;
			Pages = values.Pages;
		}

		private List<T> DataList { get; }
		public override object Data => DataList;
		[JsonPropertyName("inPage")] public int InPage => DataList.Count;
		[JsonPropertyName("page")] public int Page { get; }
		[JsonPropertyName("pageSize")] public int PageSize { get; }
		[JsonPropertyName("pages")] public int Pages { get; }
		[JsonPropertyName("count")] public long Count { get; }
	}
}