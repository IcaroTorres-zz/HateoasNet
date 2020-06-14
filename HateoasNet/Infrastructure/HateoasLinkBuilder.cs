using HateoasNet.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace HateoasNet.Infrastructure
{
    /// <inheritdoc cref="IHateoasLinkBuilder{T}" />
    public sealed class HateoasLinkBuilder<T> : IHateoasLinkBuilder<T> where T : class
    {
        internal HateoasLinkBuilder(string routeName) : this(routeName, e => null, e => true)
        {
        }

        private HateoasLinkBuilder(string routeName, Func<T, IDictionary<string, object>> routeDictionaryFunction, Func<T, bool> predicate)
        {
            RouteName = routeName;
            PresentedName = routeName;
            RouteDictionaryFunction = routeDictionaryFunction;
            Predicate = predicate;
        }

        public string RouteName { get; }
        public string PresentedName { get; private set; }
        public Func<T, bool> Predicate { get; private set; }
        public Func<T, IDictionary<string, object>> RouteDictionaryFunction { get; private set; }

        public IDictionary<string, object> GetRouteDictionary(object source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return RouteDictionaryFunction((T)source);
        }

        public bool IsApplicable(object source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return Predicate((T)source);
        }

        public IHateoasLinkBuilder<T> HasRouteData(Func<T, object> routeDataFunction)
        {
            if (routeDataFunction == null) throw new ArgumentNullException(nameof(routeDataFunction));
            RouteDictionaryFunction = source => ToRouteDictionary(routeDataFunction(source));
            return this;
        }

        public IHateoasLinkBuilder<T> When(Func<T, bool> predicate)
        {
            Predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
            return this;
        }

        public IHateoasLinkBuilder<T> PresentedAs(string presentedName)
        {
            if (!string.IsNullOrWhiteSpace(presentedName)) PresentedName = presentedName;
            return this;
        }

        private IDictionary<string, object> ToRouteDictionary(object source)
        {
            if (source is IEnumerable) return new Dictionary<string, object>();

            static string NameFunction(MemberInfo info)
            {
                return info.Name;
            }

            object ValueFunction(PropertyInfo info)
            {
                return info.GetValue(source, BindingFlags.Public, null, null, CultureInfo.InvariantCulture);
            }

            return source.GetType()
                         .GetProperties()
                         .Where(x => x.CanRead && x.MemberType == MemberTypes.Property)
                         .ToDictionary(NameFunction, ValueFunction, StringComparer.OrdinalIgnoreCase);
        }
    }
}
