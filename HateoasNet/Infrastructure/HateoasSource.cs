using HateoasNet.Abstractions;
using System;
using System.Collections.Generic;

namespace HateoasNet.Infrastructure
{
    /// <inheritdoc cref="IHateoasSource" />
    public sealed class HateoasSource<T> : IHateoasSource<T> where T : class
    {
        private readonly List<IHateoasLinkBuilder> _linkBuilders = new List<IHateoasLinkBuilder>();

        internal HateoasSource()
        {
        }

        public IEnumerable<IHateoasLinkBuilder> GetLinkBuilders()
        {
            return _linkBuilders;
        }

        public IHateoasLinkBuilder<T> AddLink(string routeName)
        {
            if (string.IsNullOrWhiteSpace(routeName)) throw new ArgumentNullException(nameof(routeName));
            var linkBuilder = new HateoasLinkBuilder<T>(routeName);
            _linkBuilders.Add(linkBuilder);
            return linkBuilder;
        }
    }
}
