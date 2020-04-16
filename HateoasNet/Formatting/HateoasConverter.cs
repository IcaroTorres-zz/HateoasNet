using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HateoasNet.Abstractions;
using HateoasNet.Resources;
#if !NET472
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
#if NETCOREAPP3_1
using Microsoft.AspNetCore.Mvc.ActionConstraints;

#elif NETSTANDARD2_0
using Microsoft.AspNetCore.Mvc.Internal;
#endif

namespace HateoasNet.Formatting
{
	public class HateoasConverter : IHateoasConverter
	{
		private readonly HttpContext _httpContext;

		public HateoasConverter(IHttpContextAccessor httpContextAccessor)
		{
			_httpContext = httpContextAccessor.HttpContext;
		}

		public PaginationResource<Resource> ToPaginationResource(object source, Type objectType)
		{
			var itemType = objectType.GetGenericArguments().First();
			var (items, count, pageSize, page) = GetPaginationProperties(source);
			var singleResources = items.Select(item => ToSingleResource(item, itemType));
			var resourcePagination = new Pagination<Resource>(singleResources, count, pageSize, page);

			return Convert(resourcePagination, objectType);
		}

		public EnumerableResource<Resource> ToEnumerableResource(object source, Type objectType)
		{
			var enumerable = source as IEnumerable<object> ?? throw new InvalidCastException(
				$"Cannot cast from {source.GetType().Name} to {typeof(IEnumerable<object>).Name}.");

			var itemType = objectType.GetGenericArguments().First();
			var singleResources = enumerable.Select(item => ToSingleResource(item, itemType));
			return Convert(singleResources, objectType);
		}

		public SingleResource ToSingleResource(object source, Type type)
		{
			var singleResource = new SingleResource(source);
			return (SingleResource) ApplyHateoasLinks(singleResource, type, _httpContext);
		}

		internal PaginationResource<Resource> Convert(Pagination<Resource> source, Type type)
		{
			var paginationResource = new PaginationResource<Resource>(source);
			return (PaginationResource<Resource>) ApplyHateoasLinks(paginationResource, type, _httpContext);
		}

		internal EnumerableResource<Resource> Convert(IEnumerable<Resource> source, Type type)
		{
			var enumerableResource = new EnumerableResource<Resource>(source);
			return (EnumerableResource<Resource>) ApplyHateoasLinks(enumerableResource, type, _httpContext);
		}

		// TODO: implement .Net Framework version of this method and refactor classes with Base and derived ones for each platform 
		internal Resource ApplyHateoasLinks(Resource resource, Type sourceType, HttpContext context)
		{
			var urlHelperFactory = context.RequestServices.GetRequiredService<IUrlHelperFactory>();
			var actionContextAccessor = context.RequestServices.GetRequiredService<IActionContextAccessor>();
			var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
			var actionDescriptors = context.RequestServices
				.GetRequiredService<IActionDescriptorCollectionProvider>()
				.ActionDescriptors.Items;
			var configuration = context.RequestServices.GetRequiredService<IHateoasConfiguration>();

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

		internal (IEnumerable<object>, long, int, int) GetPaginationProperties(object source)
		{
			var invalidProperties = new List<string>();
			var paginationTupleProperties = (default(IEnumerable<object>), default(long), default(int), default(int));

			if (source.GetType().GetProperty(nameof(Pagination<object>.Data)) is {} itemsInfo)
				paginationTupleProperties.Item1 = (IEnumerable<object>) itemsInfo.GetValue(source);
			else invalidProperties.Add(nameof(Pagination<object>.Data));

			if (source.GetType().GetProperty(nameof(Pagination<object>.Count)) is {} countInfo)
				paginationTupleProperties.Item2 = (long) countInfo.GetValue(source);
			else invalidProperties.Add(nameof(Pagination<object>.Count));

			if (source.GetType().GetProperty(nameof(Pagination<object>.PageSize)) is {} pageSizeInfo)
				paginationTupleProperties.Item3 = (int) pageSizeInfo.GetValue(source);
			else invalidProperties.Add(nameof(Pagination<object>.PageSize));

			if (source.GetType().GetProperty(nameof(Pagination<object>.Page)) is {} pageInfo)
				paginationTupleProperties.Item4 = (int) pageInfo.GetValue(source);
			else invalidProperties.Add(nameof(Pagination<object>.Page));

			if (!invalidProperties.Any()) return paginationTupleProperties;

			var properties = string.Join(", ", invalidProperties);
			throw new TargetException(
				$"Unable to create '{nameof(Pagination<object>)}' from '{source.GetType().Name}'. Properties [{properties}] not found.");
		}
	}
}
#else
namespace HateoasNet.Formatting
{
	public class HateoasConverter : IHateoasConverter
	{
		// TODO: added for temporary use on net full implementations. will be removed
		public HateoasConverter()
		{
		}

		public PaginationResource<Resource> ToPaginationResource(object source, Type objectType)
		{
			var itemType = objectType.GetGenericArguments().First();
			var (items, count, pageSize, page) = GetPaginationProperties(source);
			var singleResources = items.Select(item => ToSingleResource(item, itemType));
			var resourcePagination = new Pagination<Resource>(singleResources, count, pageSize, page);

			return new PaginationResource<Resource>(resourcePagination);
		}

		public EnumerableResource<Resource> ToEnumerableResource(object source, Type objectType)
		{
			var enumerable = source as IEnumerable<object> ?? throw new InvalidCastException(
				$"Cannot cast from {source.GetType().Name} to {typeof(IEnumerable<object>).Name}.");

			var itemType = objectType.GetGenericArguments().First();
			var singleResources = enumerable.Select(item => ToSingleResource(item, itemType));
			return new EnumerableResource<Resource>(singleResources);
		}

		public SingleResource ToSingleResource(object source, Type type)
		{
			return new SingleResource(source);
		}

internal (IEnumerable<object>, long, int, int) GetPaginationProperties(object source)
		{
			var invalidProperties = new List<string>();
			var paginationTupleProperties = (default(IEnumerable<object>), default(long), default(int), default(int));

			if (source.GetType().GetProperty(nameof(Pagination<object>.Data)) is {} itemsInfo)
				paginationTupleProperties.Item1 = (IEnumerable<object>) itemsInfo.GetValue(source);
			else invalidProperties.Add(nameof(Pagination<object>.Data));

			if (source.GetType().GetProperty(nameof(Pagination<object>.Count)) is {} countInfo)
				paginationTupleProperties.Item2 = (long) countInfo.GetValue(source);
			else invalidProperties.Add(nameof(Pagination<object>.Count));

			if (source.GetType().GetProperty(nameof(Pagination<object>.PageSize)) is {} pageSizeInfo)
				paginationTupleProperties.Item3 = (int) pageSizeInfo.GetValue(source);
			else invalidProperties.Add(nameof(Pagination<object>.PageSize));

			if (source.GetType().GetProperty(nameof(Pagination<object>.Page)) is {} pageInfo)
				paginationTupleProperties.Item4 = (int) pageInfo.GetValue(source);
			else invalidProperties.Add(nameof(Pagination<object>.Page));

			if (!invalidProperties.Any()) return paginationTupleProperties;
			
			var properties = string.Join(", ", invalidProperties);
			throw new TargetException(
				$"Unable to create '{nameof(Pagination<object>)}' from '{source.GetType().Name}'. Properties [{properties}] not found.");
		}
	}
}
#endif