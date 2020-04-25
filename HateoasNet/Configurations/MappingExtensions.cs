using System.Collections.Generic;
using System.Linq;

namespace HateoasNet.Configurations
{
	public static class MappingExtensions
	{
		public static IDictionary<string, object> ToRouteDictionary(this object source)
		{
			return source.GetType().GetProperties()
			             .Where(propertyInfo => propertyInfo.CanRead)
			             .ToDictionary(p => p.Name, p => p.GetValue(source));
		}
	}
}