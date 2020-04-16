using System;
using System.Collections.Generic;
using HateoasNet.Abstractions;

namespace HateoasNet.Mapping
{
	public abstract class AbstractHateoasLink<T> : IHateoasLink<T> where T : class
	{
		protected internal AbstractHateoasLink(string routeName) : this(routeName, e => null, e => true)
		{
		}

		private AbstractHateoasLink(string routeName,
			Func<T, IDictionary<string, object>> routeDictionaryFunction,
			Func<T, bool> predicateFunction)
		{
			RouteName = routeName;
			RouteDictionaryFunction = routeDictionaryFunction;
			PredicateFunction = predicateFunction;
		}

		public Func<T, bool> PredicateFunction { get; protected set; }
		public Func<T, IDictionary<string, object>> RouteDictionaryFunction { get; protected set; }
		public string RouteName { get; protected set; }

		public virtual IDictionary<string, object> GetRouteDictionary(object routeData)
		{
			if (routeData == null) throw new ArgumentNullException(nameof(routeData));

			return RouteDictionaryFunction(routeData as T);
		}

		public virtual bool IsDisplayable(object routeData)
		{
			if (routeData == null) throw new ArgumentNullException(nameof(routeData));

			return PredicateFunction(routeData as T);
		}

		public abstract IHateoasLink<T> HasRouteData(Func<T, object> routeDataFunction);

		public virtual IHateoasLink<T> HasConditional(Func<T, bool> predicate)
		{
			PredicateFunction = predicate ?? throw new ArgumentNullException(nameof(predicate));
			return this;
		}
	}
}