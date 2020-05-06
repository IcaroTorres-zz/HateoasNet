using HateoasNet.Abstractions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HateoasNet.Resources
{
    /// <inheritdoc cref="IPagination{T}" />
    [System.Serializable]
    public class Pagination<T> : IPagination<T> where T : class
    {
        public Pagination(IEnumerable<T> data, long count, int pageSize, int page = 1)
        {
            Items = data.ToList();
            Count = count;
            PageSize = pageSize;
            Page = page;
        }

        public virtual IEnumerable<T> Items { get; }
        public virtual long Count { get; }
        public virtual int PageSize { get; }
        public virtual int Page { get; }
        public virtual int Pages => (int)(Count == 0 ? 1 : (Count + PageSize - 1) / PageSize);

        public virtual IEnumerable<T> GetItems() => Items;

        IEnumerable IPagination.GetItems() => GetItems();
    }
}
