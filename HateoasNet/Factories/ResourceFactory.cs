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

		public SingleResource Create(object source, Type type)
		{
			var singleResource = new SingleResource(source);
			BuildResourceLinks(singleResource, source, type);
			return singleResource;
		}

		public EnumerableResource Create(IEnumerable source, Type type)
		{
			var enumeration = EnumerateToResources(source, type);
			var enumeratedResource = new EnumerableResource(enumeration);
			BuildResourceLinks(enumeratedResource, source, type);
			return enumeratedResource;
		}

		public PaginationResource Create(IPagination source, Type type)
		{
			var singleResources = EnumerateToResources(source.GetEnumeration(), type);
			var resourcesPagination = new Pagination<Resource>(singleResources, source.Count, source.PageSize, source.Page);
			var paginationResource = new PaginationResource(resourcesPagination);
			BuildResourceLinks(paginationResource, source, type);
			return paginationResource;
		}

		public void BuildResourceLinks(Resource resource, object data, Type type)
		{
			foreach (var hateoasLink in _hateoasConfiguration.GetApplicableLinks(type, data))
			{
				var createdLink =
					_resourceLinkFactory.Create(hateoasLink.RouteName, hateoasLink.GetRouteDictionary(data));

				resource.Links.Add(createdLink);
			}
		}

		internal IEnumerable<Resource> EnumerateToResources(IEnumerable source, Type type)
		{
			return from object item in source select Create(item, type.GetGenericArguments().First());
		}
	}
}
