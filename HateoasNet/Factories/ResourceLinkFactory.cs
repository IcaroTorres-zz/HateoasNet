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

		public ResourceLink Create(string rel, IDictionary<string, object> routeValuesDictionary)
		{
			if (string.IsNullOrWhiteSpace(rel)) throw new ArgumentNullException(nameof(rel));

			var href = _urlBuilder.Build(rel, routeValuesDictionary);
			var method = _httpMethodFinder.Find(rel);
			return new ResourceLink(rel, href, method);
		}
	}
}
