using HateoasNet.Abstractions;
using HateoasNet.Extensions;
using System;
using System.Collections.Generic;

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
            RouteDictionaryFunction = source => routeDataFunction(source).ToRouteDictionary();
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
    }
}
