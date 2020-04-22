using System.Collections.Generic;
using System.Linq;
using HateoasNet.Abstractions;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace HateoasNet.Core.Resources
{
	public class HttpMethodFinder : IHttpMethodFinder
	{
		private readonly IReadOnlyList<ActionDescriptor> _actionDescriptors;

		public HttpMethodFinder(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
		{
			_actionDescriptors = actionDescriptorCollectionProvider.ActionDescriptors.Items;
		}

		public string Find(string routeName)
		{
			return _actionDescriptors.SingleOrDefault(x => x.AttributeRouteInfo.Name == routeName)
				?.ActionConstraints.OfType<HttpMethodActionConstraint>().First().HttpMethods.First();
		}
	}
}