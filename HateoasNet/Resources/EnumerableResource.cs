using System.Collections.Generic;
using System.Linq;

namespace HateoasNet.Resources
{
	/// <summary>
	///   Represents an formatted enumeration wrapper of <see cref="Resource" /> wrapper items which inherit from
	///   <see cref="Resource" />.
	/// </summary>
	public class EnumerableResource<T> : Resource where T : Resource
	{
		public EnumerableResource(IEnumerable<T> data) : base(data)
		{
			EnumerableData = data.ToArray();
		}

		private IEnumerable<T> EnumerableData { get; }

		/// <summary>
		///   The <see cref="IEnumerable{Resource}" /> items as <see cref="object" />.
		/// </summary>
		public override object Data => EnumerableData;
	}
}