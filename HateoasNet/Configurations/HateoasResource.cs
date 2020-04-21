using System;
using System.Collections.Generic;
using HateoasNet.Abstractions;

namespace HateoasNet.Configurations
{
	/// <inheritdoc cref="IHateoasResource"/>
	public sealed class HateoasResource<T> : IHateoasResource<T> where T : class
	{
		internal HateoasResource()
		{
		}

		private readonly Dictionary<string, IHateoasLink> _hateoasLinks = new Dictionary<string, IHateoasLink>();

		public IEnumerable<IHateoasLink> GetLinks() => _hateoasLinks.Values;

		public IHateoasLink<T> HasLink(string routeName)
		{
			if (routeName == null) throw new ArgumentNullException(nameof(routeName));

			_hateoasLinks[routeName] = new HateoasLink<T>(routeName);

			return _hateoasLinks[routeName] as IHateoasLink<T>;
		}
	}
}