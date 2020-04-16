using System;
using System.Collections.Generic;
using HateoasNet.Abstractions;

namespace HateoasNet.Mapping
{
	public class HateoasMap<T> : IHateoasMap<T> where T : class
	{
		private readonly Dictionary<string, IHateoasLink> _hateoasLinks = new Dictionary<string, IHateoasLink>();

		public IEnumerable<IHateoasLink> GetLinks()
		{
			return _hateoasLinks.Values;
		}

		public IHateoasLink<T> HasLink(string routeName)
		{
			if (routeName == null) throw new ArgumentNullException(nameof(routeName));
			
			_hateoasLinks[routeName] = new HateoasLink<T>(routeName);

			return _hateoasLinks[routeName] as IHateoasLink<T>;
		}
	}
}