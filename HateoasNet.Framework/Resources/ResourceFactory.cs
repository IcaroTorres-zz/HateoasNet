using System;
using HateoasNet.Abstractions;
using HateoasNet.Resources;

namespace HateoasNet.Framework.Resources
{
	public class ResourceFactory : AbstractResourceFactory
	{
		private readonly IHateoasConfiguration _hateoasConfiguration;
		private readonly IResourceLinkFactory _resourceLinkFactory;

		public ResourceFactory(IHateoasConfiguration hateoasConfiguration, IResourceLinkFactory resourceLinkFactory)
		{
			_hateoasConfiguration = hateoasConfiguration;
			_resourceLinkFactory = resourceLinkFactory;
		}

		protected override Resource ApplyHateoasLinks(Resource resource, Type sourceType)
		{
			foreach (var hateoasLink in _hateoasConfiguration.GetMappedLinks(sourceType, resource.Data))
			{
				var createdLink = _resourceLinkFactory.Create(resource, hateoasLink);

				resource.Links.Add(createdLink);
			}

			return resource;
		}
	}
}