using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace HateoasNet.Resources
{
	public class PaginationResource<T> : Resource where T : Resource
	{
		public PaginationResource(Pagination<T> values) : base(values.Data)
		{
			Page = values.Page;
			PageSize = values.PageSize;
			Pages = values.Pages;
			Count = values.Count;
			DataList = values.Data.ToList();
		}

		private List<T> DataList { get; }
		public override object Data => DataList;
		[JsonPropertyName("inPage")] public long InPage => DataList.Count;
		[JsonPropertyName("page")] public long Page { get; private set; }
		[JsonPropertyName("pageSize")] public long PageSize { get; private set; }
		[JsonPropertyName("pages")] public long Pages { get; private set; }
		[JsonPropertyName("count")] public long Count { get; private set; }
	}
}