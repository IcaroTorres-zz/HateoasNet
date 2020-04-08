using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;

namespace HateoasNet
{
	public class HateoasNetOptions
	{
		private readonly List<IHateoasLink> _links = new List<IHateoasLink>();
		public IEnumerable<IHateoasLink> Links => _links.AsReadOnly();

		public HateoasNetOptions AddLink<T>(string routeName, Func<T, object> objectFunction = null,
			Func<T, bool> predicate = null) where T : class
		{
			var valuesFunction = new Func<T, RouteValueDictionary>(
				sourceValue => new RouteValueDictionary(
					(objectFunction ?? (e => null))(sourceValue)
				)
			);

			_links.Add(new HateoasLink<T>(routeName, valuesFunction, predicate));
			return this;
		}
	}
}