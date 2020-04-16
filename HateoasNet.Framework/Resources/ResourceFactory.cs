using System;
using System.Web.Http.Routing;
using HateoasNet.Abstractions;
using HateoasNet.Resources;

namespace HateoasNet.Framework.Resources
{
	public class ResourceFactory : AbstractResourceFactory
	{
		private readonly IHateoasConfiguration _hateoasConfiguration;
		private readonly UrlHelper _urlHelper;

		public ResourceFactory(IHateoasConfiguration hateoasConfiguration, UrlHelper urlHelper)
		{
			_hateoasConfiguration = hateoasConfiguration;
			_urlHelper = urlHelper;
		}
		
		protected override Resource ApplyHateoasLinks(Resource resource, Type sourceType)
		{
			foreach (var link in _hateoasConfiguration.GetMappedLinks(sourceType, resource.Data))
			{
				var method = _urlHelper.Request.Method.ToString();
				var url = _urlHelper.Link(link.RouteName, link.GetRouteDictionary(resource.Data));
				resource.Links.Add(new ResourceLink(link.RouteName, url, method));
			}

			return resource;
		}
	}
}