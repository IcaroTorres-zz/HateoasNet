using System;
using System.Collections.Generic;
using HateoasNet.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
#if NETCOREAPP3_1
using Microsoft.AspNetCore.Mvc.ActionConstraints;
#elif NETSTANDARD2_0
using Microsoft.AspNetCore.Mvc.Internal;
#endif
using System.Linq;
using HateoasNet.Abstractions;
using HateoasNet.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HateoasNet.Formatting
{
	public class HateoasConverter : IHateoasConverter
	{
		private readonly HttpContext _httpContext;

		public HateoasConverter(IHttpContextAccessor httpContextAccessor)
		{
			_httpContext = httpContextAccessor.HttpContext;
		}

		public PaginationResource<Resource> ToPaginationResource(object value, Type objectType)
		{
			var originalPagination = ToPagination(value);
			var itemType = objectType.GetGenericArguments().First();

			var singleResources = originalPagination.Data.Select(item => ToSingleResource(item, itemType));

			var resourcePagination = new Pagination<Resource>(singleResources,
				originalPagination.Count,
				originalPagination.PageSize,
				originalPagination.Page);

			return Convert(resourcePagination, objectType);
		}

		public EnumerableResource<Resource> ToEnumerableResource(object value, Type objectType)
		{
			var enumerable = value as IEnumerable<object> ??
			                 throw new InvalidCastException(
				                 $"Cannot cast from {value.GetType().Name} to {typeof(IEnumerable<object>).Name}.");

			var itemType = objectType.GetGenericArguments().First();
			var singleResources = enumerable.Select(item => ToSingleResource(item, itemType));
			return Convert(singleResources, objectType);
		}

		public SingleResource ToSingleResource(object source, Type type)
		{
			var singleResource = new SingleResource(source);
			return (SingleResource) ApplyHateoasLinks(singleResource, type, _httpContext);
		}

		private PaginationResource<Resource> Convert(Pagination<Resource> source, Type type)
		{
			var paginationResource = new PaginationResource<Resource>(source);
			return (PaginationResource<Resource>) ApplyHateoasLinks(paginationResource, type, _httpContext);
		}

		private EnumerableResource<Resource> Convert(IEnumerable<Resource> source, Type type)
		{
			var enumerableResource = new EnumerableResource<Resource>(source);
			return (EnumerableResource<Resource>) ApplyHateoasLinks(enumerableResource, type, _httpContext);
		}

		private Resource ApplyHateoasLinks(Resource resource, Type sourceType, HttpContext context)
		{
			var urlHelperFactory = context.RequestServices.GetRequiredService<IUrlHelperFactory>();
			var actionContextAccessor = context.RequestServices.GetRequiredService<IActionContextAccessor>();
			var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
			var actionDescriptors = context.RequestServices
				.GetRequiredService<IActionDescriptorCollectionProvider>()
				.ActionDescriptors.Items;
			var configuration = context.RequestServices.GetRequiredService<IOptions<HateoasConfiguration>>().Value;

			foreach (var link in configuration.GetMappedLinks(sourceType, resource.Data))
			{
				var route = actionDescriptors.SingleOrDefault(x => x.AttributeRouteInfo.Name == link.RouteName);
				var url = urlHelper.Link(link.RouteName, link.GetRouteDictionary(resource.Data))?.ToLower();

				if (!(route is { }) || !(url is { })) continue;

				var method = route.ActionConstraints.OfType<HttpMethodActionConstraint>().First().HttpMethods.First();
				resource.Links.Add(new ResourceLink(link.RouteName, url, method));
			}

			return resource;
		}

		private Pagination<object> ToPagination(object source)
		{
			var items = (IEnumerable<object>) source.GetType().GetProperty(nameof(Pagination<object>.Data)).GetValue(source);
			var count = (long) source.GetType().GetProperty(nameof(Pagination<object>.Count)).GetValue(source);
			var pageSize = (int) source.GetType().GetProperty(nameof(Pagination<object>.PageSize)).GetValue(source);
			var page = (int) source.GetType().GetProperty(nameof(Pagination<object>.Page)).GetValue(source);
			return new Pagination<object>(items, count, pageSize, page);
		}
	}
}