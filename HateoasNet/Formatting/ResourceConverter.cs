using System;
using System.Collections.Generic;
using HateoasNet.Resources;
using Microsoft.AspNetCore.Http;

namespace HateoasNet.Formatting
{
	public class ResourceConverter
	{
		private readonly HttpContext _httpContext;

		public ResourceConverter(HttpContext httpContext)
		{
			_httpContext = httpContext;
		}

		public PaginationResource<Resource> ToPagination(Pagination<Resource> source, Type type)
		{
			return (PaginationResource<Resource>) new PaginationResource<Resource>(source)
				.ApplyHateoasLinks(type, _httpContext);
		}

		public EnumerableResource<Resource> ToEnumerable(IEnumerable<Resource> source, Type type)
		{
			return (EnumerableResource<Resource>) new EnumerableResource<Resource>(source)
				.ApplyHateoasLinks(type, _httpContext);
		}

		public SingleResource ToSingle(object source, Type type)
		{
			return (SingleResource) new SingleResource(source).ApplyHateoasLinks(type, _httpContext);
		}
	}
}