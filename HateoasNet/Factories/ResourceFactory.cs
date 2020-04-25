using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HateoasNet.Abstractions;
using HateoasNet.Resources;

namespace HateoasNet.Factories
{
	/// <inheritdoc cref="IResourceFactory" />
	public sealed class ResourceFactory : IResourceFactory
	{
		private readonly IHateoasContext _hateoasConfiguration;
		private readonly IResourceLinkFactory _resourceLinkFactory;

		public ResourceFactory(IHateoasContext hateoasConfiguration, IResourceLinkFactory resourceLinkFactory)
		{
			_hateoasConfiguration = hateoasConfiguration;
			_resourceLinkFactory = resourceLinkFactory;
		}

		public Resource Create(object source, Type type)
		{
			return BuildResourceLinks(new SingleResource(source), type);
		}

		public Resource Create(IEnumerable source, Type type)
		{
			var enumeration = EnumerateToResources(source, type);
			var enumeratedResource = new EnumerableResource<Resource>(enumeration);

			return BuildResourceLinks(enumeratedResource, type);
		}

		public Resource Create(IPagination source, Type type)
		{
			var singleResources = EnumerateToResources(source.GetEnumeration(), type);
			var resourcesPagination = new Pagination<Resource>(singleResources, source.Count, source.PageSize, source.Page);
			var paginationResource = new PaginationResource<Resource>(resourcesPagination);

			return BuildResourceLinks(paginationResource, type);
		}

		public Resource BuildResourceLinks(Resource resource, Type type)
		{
			foreach (var hateoasLink in _hateoasConfiguration.GetApplicableLinks(type, resource.Data))
			{
				var createdLink =
					_resourceLinkFactory.Create(hateoasLink.RouteName, hateoasLink.GetRouteDictionary(resource.Data));

				resource.Links.Add(createdLink);
			}

			return resource;
		}

		private IEnumerable<Resource> EnumerateToResources(IEnumerable source, Type type)
		{
			return from object item in source select Create(item, type.GetGenericArguments().First());
		}
	}
}