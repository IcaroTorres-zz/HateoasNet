using System.Collections.Generic;
using System.Linq;
using HateoasNet.Abstractions;

namespace HateoasNet.Resources
{
	/// /// <summary>
	/// Represents an formatted pagination wrapper of <see cref="Resource"/> wrapper items which inherit from <see cref="Resource"/>.
	/// </summary>
	public class PaginationResource<T> : Resource where T : Resource
	{
		public PaginationResource(IPagination<T> values) : base(values.Data)
		{
			EnumerableData = values.Data;
			Count = values.Count;
			PageSize = values.PageSize;
			Page = values.Page;
			Pages = values.Pages;
		}

		private IEnumerable<T> EnumerableData { get; }
		
		/// <summary>
		/// The <see cref="IEnumerable{Resource}"/> items as <see cref="object"/>.
		/// </summary>
		public override object Data => EnumerableData;
		public int InPage => EnumerableData.Count();
		public int Page { get; }
		public int PageSize { get; }
		public int Pages { get; }
		public long Count { get; }
	}
}