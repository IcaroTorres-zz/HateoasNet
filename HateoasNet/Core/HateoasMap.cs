using System;
using System.Collections.Generic;
using HateoasNet.Core.Abstractions;
using Microsoft.AspNetCore.Routing;

namespace HateoasNet.Core
{
	public sealed class HateoasMap<T> : IHateoasMap where T : class
	{
		private readonly List<IHateoasLink> _hateoasLinks = new List<IHateoasLink>();
		public IReadOnlyList<IHateoasLink> GetLinks() => _hateoasLinks.AsReadOnly();

		public IHateoasLink<T> HasLink(string routeName)
		{
			var routeDataFunction = new Func<T, object>(e => null);
			var predicate = new Func<T, bool>(e => true);
			return HasLink(routeName, routeDataFunction, predicate);
		}

		public IHateoasLink<T> HasLink(string routeName, Func<T, object> routeDataFunction, Func<T, bool> predicate = null)
		{
			if (routeDataFunction == null) throw new ArgumentNullException(nameof(routeDataFunction));

			var valuesFunction = new Func<T, RouteValueDictionary>(
				e => new RouteValueDictionary(routeDataFunction(e)));

			predicate ??= t => true;

			var hateoasLink = new HateoasLink<T>(routeName, valuesFunction, predicate);
			_hateoasLinks.Add(hateoasLink);
			return hateoasLink;
		}
	}
}