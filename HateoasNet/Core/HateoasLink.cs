using System;
using HateoasNet.Abstractions;
using Microsoft.AspNetCore.Routing;

namespace HateoasNet.Core
{
	public sealed class HateoasLink<T> : IHateoasLink<T> where T : class
	{
		private Func<T, bool> _predicateFunction;
		private Func<T, RouteValueDictionary> _valuesFunction;

		internal HateoasLink(string routeName, Func<T, RouteValueDictionary> valuesFunction, Func<T, bool> predicate)
		{
			RouteName = routeName;
			_valuesFunction = valuesFunction;
			_predicateFunction = predicate;
		}

		public string RouteName { get; }

		public RouteValueDictionary GetRouteDictionary(object routeData) => _valuesFunction(routeData as T);

		public bool IsDisplayable(object routeData) => _predicateFunction(routeData as T);

		public IHateoasLink<T> HasRouteData(Func<T, object> routeDataFunction)
		{
			if (routeDataFunction == null) throw new ArgumentNullException(nameof(routeDataFunction));

			_valuesFunction = (T source) => new RouteValueDictionary(routeDataFunction(source));
			return this;
		}

		public IHateoasLink<T> HasConditional(Func<T, bool> predicate)
		{
			_predicateFunction = predicate ?? throw new ArgumentNullException(nameof(predicate));
			return this;
		}
	}
}