using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;

namespace HateoasNet.Core
{
	public class HateoasNetOptions
	{
		private readonly List<IHateoasLink> _links = new List<IHateoasLink>();
		public IReadOnlyList<IHateoasLink> Links => _links.AsReadOnly();

		public IHateoasLink<T> EnableLinkFor<T>(string routeName, 
			Func<T, object> objectFunction = null,
			Func<T, bool> predicate = null) where T : class
		{
			var valuesFunction = new Func<T, RouteValueDictionary>(
				sourceValue => new RouteValueDictionary(
					(objectFunction ?? (e => null))(sourceValue)
				)
			);
			predicate ??= t => true;
			
			var hateoasLink = new HateoasLink<T>(routeName, valuesFunction, predicate);
			_links.Add(hateoasLink);
			return hateoasLink;
		}
	}
}