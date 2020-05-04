using HateoasNet.Abstractions;
using HateoasNet.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HateoasNet.Core.Factories
{
    /// <inheritdoc cref="IResourceLinkFactory" />
    public sealed class ResourceLinkFactory : IResourceLinkFactory
    {
        private readonly IReadOnlyList<ActionDescriptor> _actionDescriptors;
        private readonly IUrlHelper _urlHelper;

        public ResourceLinkFactory(IUrlHelper urlHelper, IActionDescriptorCollectionProvider actionDescriptorsProvider)
        {
            _urlHelper = urlHelper;
            _actionDescriptors = actionDescriptorsProvider.ActionDescriptors.Items;
        }

        public ResourceLink Create(string rel, IDictionary<string, object> routeValues)
        {
            if (string.IsNullOrWhiteSpace(rel)) throw new ArgumentNullException(nameof(rel));
            if (!TryGetActionDescriptorByRouteName(rel, out var actionDescriptor))
            {
                throw new NotSupportedException($"Unable to find route '{rel}' and respective {nameof(ActionDescriptor)}");
            }

            var href = _urlHelper.Link(rel, routeValues);
            var method = actionDescriptor.ActionConstraints.OfType<HttpMethodActionConstraint>().First().HttpMethods.First();
            return new ResourceLink(rel, href, method);
        }

        private bool TryGetActionDescriptorByRouteName(string routeName, out ActionDescriptor descriptor)
        {
            descriptor = _actionDescriptors.SingleOrDefault(x => x.AttributeRouteInfo.Name == routeName);

            return descriptor != null;
        }
    }
}
