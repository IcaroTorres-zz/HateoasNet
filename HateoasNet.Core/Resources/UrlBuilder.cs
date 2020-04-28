using System;
using System.Collections.Generic;
using HateoasNet.Abstractions;
using HateoasNet.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace HateoasNet.Core.Resources
{
	public class UrlBuilder : IUrlBuilder
	{
		private readonly IUrlHelper _urlHelper;

		public UrlBuilder(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
		{
			_urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
		}

		public string Build(string routeName, IDictionary<string, object> routeDictionary)
		{
			if (string.IsNullOrWhiteSpace(routeName)) throw new ArgumentNullException(nameof(routeName));

			return _urlHelper.Link(routeName, routeDictionary);
		}
	}
}