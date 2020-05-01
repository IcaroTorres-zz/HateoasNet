using System;
using System.Collections.Generic;
using System.Linq;
using HateoasNet.Abstractions;
using HateoasNet.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace HateoasNet.Core.Factories
{
	/// <inheritdoc cref="IResourceLinkFactory" />
	public sealed class ResourceLinkFactory : IResourceLinkFactory
	{
		private readonly ActionDescriptor _actionDescriptor;
		private readonly IUrlHelper _urlHelper;

		public ResourceLinkFactory(IUrlHelper urlHelper, IActionContextAccessor actionContextAccessor)
		{
			_urlHelper = urlHelper;
			_actionDescriptor = actionContextAccessor.ActionContext.ActionDescriptor;
		}

		public ResourceLink Create(string rel, IDictionary<string, object> routeValues)
		{
			if (string.IsNullOrWhiteSpace(rel)) throw new ArgumentNullException(nameof(rel));

			var href = _urlHelper.Link(rel, routeValues);
			var method = _actionDescriptor.ActionConstraints.OfType<HttpMethodActionConstraint>().First().HttpMethods.First();
			return new ResourceLink(rel, href, method);
		}
	}
}
