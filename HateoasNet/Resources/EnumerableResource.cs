using System.Collections.Generic;

namespace HateoasNet.Resources
{
	/// <summary>
	///   Represents an formatted enumeration wrapper of <see cref="Resource" /> wrapper items which inherit from
	///   <see cref="Resource" />.
	/// </summary>
	public class EnumerableResource : Resource
	{
		public EnumerableResource(IEnumerable<Resource> data) : base(data)
		{
			EnumerableData = data;
		}

		internal IEnumerable<Resource> EnumerableData { get; }

		/// <summary>
		///   The <see cref="Resource.Data"/> property overriden to return <see cref="IEnumerable{Resource}" />.
		/// </summary>
		public override object Data => EnumerableData;
	}
}
