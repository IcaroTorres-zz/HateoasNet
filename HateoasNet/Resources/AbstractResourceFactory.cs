using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HateoasNet.Abstractions;

namespace HateoasNet.Resources
{
	public abstract class AbstractResourceFactory : IResourceFactory
	{
		public virtual Resource Create(object source, Type objectType)
		{
			return ApplyLinks(new SingleResource(source), objectType);
		}

		public virtual Resource Create(IEnumerable source, Type objectType)
		{
			return ApplyLinks(EnumerateToResources(source, objectType), objectType);
		}
		
		public virtual Resource Create(IPagination source, Type objectType)
		{
			var singleResources = EnumerateToResources(source.Enumeration, objectType);
			var resourcesPagination = new Pagination<Resource>(singleResources, source.Count, source.PageSize, source.Page);
			
			return ApplyLinks(resourcesPagination, objectType);
		}

		public virtual Resource ApplyLinks(Resource source, Type type)
		{
			return ApplyHateoasLinks(source, type);
		}

		public virtual Resource ApplyLinks(IEnumerable<Resource> source, Type type)
		{
			var enumerableResource = new EnumerableResource<Resource>(source);
			return ApplyHateoasLinks(enumerableResource, type);
		}
		
		public virtual Resource ApplyLinks(Pagination<Resource> source, Type type)
		{
			var paginationResource = new PaginationResource<Resource>(source);
			return ApplyHateoasLinks(paginationResource, type);
		}

		private IEnumerable<Resource> EnumerateToResources(IEnumerable source, Type objectType)
		{
			return (from object item in source select Create(item, objectType.GetGenericArguments().First()));
		}

		protected abstract Resource ApplyHateoasLinks(Resource resource, Type sourceType);
	}
}