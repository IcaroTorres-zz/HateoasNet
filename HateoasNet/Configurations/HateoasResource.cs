using HateoasNet.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HateoasNet.Configurations
{
    /// <inheritdoc cref="IHateoasResource" />
    public sealed class HateoasResource<T> : IHateoasResource<T> where T : class
    {
        private readonly Dictionary<string, IHateoasLink> _hateoasLinks = new Dictionary<string, IHateoasLink>();

        internal HateoasResource()
        {
        }

        public IEnumerable<IHateoasLink> GetLinks()
        {
            return _hateoasLinks.Values.ToList();
        }

        public IHateoasLink<T> HasLink(string routeName)
        {
            if (string.IsNullOrWhiteSpace(routeName)) throw new ArgumentNullException(nameof(routeName));

            _hateoasLinks[routeName] = new HateoasLink<T>(routeName);

            return _hateoasLinks[routeName] as IHateoasLink<T>;
        }
    }
}
