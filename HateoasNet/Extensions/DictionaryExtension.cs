using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace HateoasNet.Extensions
{
    internal static class DictionaryExtension
    {
        internal static IDictionary<string, object> ToRouteDictionary(this object source)
        {
            if (source is IEnumerable) return new Dictionary<string, object>();

            static object ValueFunction(PropertyInfo info, object source)
            {
                return info.GetValue(source, BindingFlags.Public, null, null, CultureInfo.InvariantCulture);
            }

            return source.GetType()
                         .GetProperties()
                         .Where(x => x.CanRead && x.MemberType == MemberTypes.Property)
                         .ToDictionary(info => info.Name, v => ValueFunction(v, source), StringComparer.OrdinalIgnoreCase);
        }
    }
}
