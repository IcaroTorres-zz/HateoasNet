using System.Collections.Generic;
using System.Linq;

namespace HateoasNet.Resources
{
	public class EnumerableResource<T> : Resource where T : class
	{
		public EnumerableResource(IEnumerable<T> data) : base(data)
		{
			DataList = data.ToList();
		}
		private List<T> DataList { get; }
		public override object Data => DataList;
	}
}