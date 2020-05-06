using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HateoasNet.Abstractions;

namespace HateoasNet.Resources
{
    /// <summary>
    ///   Represents a formatted pagination wrapper of <see cref="Resource" /> items.
    /// </summary>
    [Serializable]
	public class PaginationResource : Resource, IPagination<Resource>
	{
		private readonly List<Resource> _items;

		public PaginationResource(IEnumerable<Resource> items, long count, int pageSize, int page) : base(items)
		{
			_items = items.ToList();
			Count = count;
			PageSize = pageSize;
			Page = page;
		}

        /// <summary>
        ///   The <see cref="Resource.Data" /> property overriden with actual value being
        ///   <see cref="IEnumerable{T}" /> of <see cref="Resource" /> items as <see langword="object" />.
        /// </summary>
        public override object Data => GetItems();

		public virtual int InPage => GetItems().Count();
		public virtual int Page { get; }
		public virtual int PageSize { get; }
		public virtual long Count { get; }
		public virtual int Pages => (int) (Count == 0 ? 1 : (Count + PageSize - 1) / PageSize);

        /// <inheritdoc cref="IPagination{Resource}.GetItems" />
        public virtual IEnumerable<Resource> GetItems()
		{
			return _items;
		}

        /// <inheritdoc cref="IPagination.GetItems" />
        IEnumerable IPagination.GetItems()
		{
			return _items;
		}
	}
}
