using HateoasNet.Abstractions;
using System;
using System.Collections.Generic;

namespace HateoasNet.Infrastructure
{
    /// <inheritdoc cref="IHateoasSource" />
    public sealed class HateoasSource<T> : IHateoasSource<T> where T : class
	{
		private readonly Dictionary<string, IHateoasLinkBuilder> _linkBuilders = new Dictionary<string, IHateoasLinkBuilder>();

		internal HateoasSource()
		{
		}

		public IEnumerable<IHateoasLinkBuilder> GetLinkBuilders()
		{
			return _linkBuilders.Values;
		}

		public IHateoasLinkBuilder<T> AddLink(string routeName)
		{
			if (string.IsNullOrWhiteSpace(routeName)) throw new ArgumentNullException(nameof(routeName));

			return (_linkBuilders[routeName] = new HateoasLinkBuilder<T>(routeName)) as IHateoasLinkBuilder<T>;
		}
	}
}
