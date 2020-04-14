using System;
using System.Collections.Generic;
using System.Linq;
using HateoasNet.Abstractions;
using Microsoft.AspNetCore.Routing;

namespace HateoasNet.Mapping
{
	public class HateoasMap<T> : IHateoasMap<T> where T : class
	{
		private readonly List<IHateoasLink> _hateoasLinks = new List<IHateoasLink>();

		public IEnumerable<IHateoasLink> GetLinks()
		{
			return _hateoasLinks.AsReadOnly().Distinct();
		}

		public IHateoasLink<T> HasLink(string routeName)
		{
			if (routeName == null) throw new ArgumentNullException(nameof(routeName));

			var routeDataFunction = new Func<T, object>(e => null);
			var predicate = new Func<T, bool>(e => true);
			return HasLink(routeName, routeDataFunction, predicate);
		}

		public IHateoasLink<T> HasLink(string routeName,
			Func<T, object> routeDataFunction,
			Func<T, bool> predicateFunction = null)
		{
			if (routeName == null) throw new ArgumentNullException(nameof(routeName));

			if (routeDataFunction == null) throw new ArgumentNullException(nameof(routeDataFunction));

			var routeDictionaryFunction =
				new Func<T, RouteValueDictionary>(e => new RouteValueDictionary(routeDataFunction(e)));

			predicateFunction ??= t => true;

			var hateoasLink = new HateoasLink<T>(routeName, routeDictionaryFunction, predicateFunction);
			_hateoasLinks.Add(hateoasLink);
			return hateoasLink;
		}
	}
}