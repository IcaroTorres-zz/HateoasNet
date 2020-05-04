using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace HateoasNet.Configurations
{
    public static class MappingExtensions
    {
        public static IDictionary<string, object> ToRouteDictionary(this object source)
        {
            if (source is IEnumerable) return new Dictionary<string, object>();

            string NameFunction(MemberInfo info) => info.Name;

            object ValueFunction(PropertyInfo info) =>
                info.GetValue(source, BindingFlags.Public, null, null, CultureInfo.InvariantCulture);

            return source.GetType()
                         .GetProperties()
                         .Where(x => x.CanRead && x.MemberType == MemberTypes.Property)
                         .ToDictionary(NameFunction, ValueFunction, StringComparer.OrdinalIgnoreCase);
        }
    }
}
