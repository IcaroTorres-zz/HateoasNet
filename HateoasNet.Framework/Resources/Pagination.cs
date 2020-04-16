using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HateoasNet.Framework.Resources
{
	public class Pagination<T> : HateoasNet.Resources.Pagination<T> where T : class
	{
		public Pagination (IEnumerable<T> data, long count, int pageSize, int page = 1) : base(data, count, pageSize, page)
		{
		}
		
		[JsonIgnore] public override IEnumerable Enumeration { get; }
	}
}