using HateoasNet.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace HateoasNet.Resources
{
    /// ///
    /// <summary>
    ///   Represents an formatted pagination wrapper of <see cref="Resource" /> wrapper items which inherit from
    ///   <see cref="Resource" />.
    /// </summary>
    public class PaginationResource : Resource
    {
        public PaginationResource(IPagination<Resource> values) : base(values.Data)
        {
            EnumerableData = values.Data;
            Count = values.Count;
            PageSize = values.PageSize;
            Page = values.Page;
        }

        public PaginationResource(IEnumerable<Resource> data, long count, int pageSize, int page) : base(data)
        {
            EnumerableData = data.ToList();
            Count = count;
            PageSize = pageSize;
            Page = page;
        }

        internal IEnumerable<Resource> EnumerableData { get; }

        /// <summary>
        ///   The <see cref="IEnumerable{Resource}" /> items as <see langword="object" />.
        /// </summary>
        public override object Data => EnumerableData;

        public int InPage => EnumerableData.Count();
        public int Page { get; }
        public int PageSize { get; }
        public long Count { get; }
        public int Pages => (int)(Count == 0 ? 1 : (Count + PageSize - 1) / PageSize);
    }
}
