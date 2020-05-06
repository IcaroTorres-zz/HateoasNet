using System.Collections.Generic;
using System.Linq;

namespace HateoasNet.Resources
{
    /// <summary>
    ///   Represents a formatted enumeration wrapper of <see cref="Resource" /> items.
    /// </summary>
    [System.Serializable]
    public class EnumerableResource : Resource
    {
        public EnumerableResource(IEnumerable<Resource> data) : base(data)
        {
            _items = data.ToList();
        }

        private readonly List<Resource> _items;


        /// <summary>
        ///   The <see cref="Resource.Data"/> property overriden with actual value being
        ///   <see cref="IEnumerable{T}" /> of <see cref="Resource"/> as <see langword="object"/>.
        /// </summary>
        public override object Data => GetItems();


        /// <summary>
        /// Returns the items as an <see cref="IEnumerable{T}" /> of <see cref="Resource"/>.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<Resource> GetItems() => _items;
    }
}
