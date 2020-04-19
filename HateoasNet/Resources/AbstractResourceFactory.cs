using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HateoasNet.Abstractions;

namespace HateoasNet.Resources
{
	public abstract class AbstractResourceFactory : IResourceFactory
	{
		public virtual Resource Create(object source, Type type)
		{
			return ApplyLinks(new SingleResource(source), type);
		}

		public virtual Resource Create(IEnumerable source, Type type)
		{
			return ApplyLinks(EnumerateToResources(source, type), type);
		}

		public virtual Resource Create(IPagination source, Type type)
		{
			var singleResources = EnumerateToResources(source.GetEnumeration(), type);
			var resourcesPagination = new Pagination<Resource>(singleResources, source.Count, source.PageSize, source.Page);

			return ApplyLinks(resourcesPagination, type);
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

		private IEnumerable<Resource> EnumerateToResources(IEnumerable source, Type type)
		{
			return from object item in source select Create(item, type.GetGenericArguments().First());
		}

		protected abstract Resource ApplyHateoasLinks(Resource resource, Type sourceType);
	}
}