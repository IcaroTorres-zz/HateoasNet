using System;
using System.Collections.Generic;
using System.Linq;
using HateoasNet.Abstractions;
using HateoasNet.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace HateoasNet.Core.Resources
{
	public class ResourceFactory : AbstractResourceFactory
	{
		private readonly IHateoasConfiguration _hateoasConfiguration;
		private readonly IUrlHelper _urlHelper;
		private readonly IReadOnlyList<ActionDescriptor> _actionDescriptors;

		public ResourceFactory(IHateoasConfiguration hateoasConfiguration,
			IUrlHelperFactory urlHelperFactory,
			IActionContextAccessor actionContextAccessor,
			IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
		{
			_hateoasConfiguration = hateoasConfiguration;
			_urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
			_actionDescriptors = actionDescriptorCollectionProvider.ActionDescriptors.Items;
		}

		protected override Resource ApplyHateoasLinks(Resource resource, Type sourceType)
		{
			foreach (var link in _hateoasConfiguration.GetMappedLinks(sourceType, resource.Data))
			{
				var route = _actionDescriptors.SingleOrDefault(x => x.AttributeRouteInfo.Name == link.RouteName);
				var url = _urlHelper.Link(link.RouteName, link.GetRouteDictionary(resource.Data))?.ToLower();

				if (!(route is { }) || !(url is { })) continue;

				var method = route.ActionConstraints.OfType<HttpMethodActionConstraint>().First().HttpMethods.First();
				resource.Links.Add(new ResourceLink(link.RouteName, url, method));
			}

			return resource;
		}
	}
}