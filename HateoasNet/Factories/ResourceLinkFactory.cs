using System;
using System.Collections.Generic;
using HateoasNet.Abstractions;
using HateoasNet.Resources;

namespace HateoasNet.Factories
{
	/// <inheritdoc cref="IResourceLinkFactory" />
	public sealed class ResourceLinkFactory : IResourceLinkFactory
	{
		private readonly IHttpMethodFinder _httpMethodFinder;
		private readonly IUrlBuilder _urlBuilder;

		public ResourceLinkFactory(IUrlBuilder urlBuilder, IHttpMethodFinder httpMethodFinder)
		{
			_urlBuilder = urlBuilder;
			_httpMethodFinder = httpMethodFinder;
		}

		public ResourceLink Create(string routeName, IDictionary<string, object> routeValuesDictionary)
		{
			if (routeName == null) throw new ArgumentNullException(nameof(routeName));

			return new ResourceLink(routeName,
			                        _urlBuilder.Build(routeName, routeValuesDictionary),
			                        _httpMethodFinder.Find(routeName));
		}
	}
}