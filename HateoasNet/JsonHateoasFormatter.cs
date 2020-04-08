using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using HateoasNet.Converters;
using HateoasNet.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace HateoasNet
{
	public class JsonHateoasFormatter : OutputFormatter
	{
		public JsonHateoasFormatter()
		{
			SupportedMediaTypes.Add("application/json");
			SupportedMediaTypes.Add("application/json+hateoas");
		}

		public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
		{
			if (context.Object is SerializableError error)
			{
				var errorOutput = JsonSerializer.Serialize(error);
				context.HttpContext.Response.ContentType = SupportedMediaTypes.First();
				return context.HttpContext.Response.WriteAsync(errorOutput);
			}

			string hateoasOutput;

			if (context.ObjectType.GetGenericTypeDefinition() == typeof(Pagination<>))
			{
				var items = context.Object.GetPropertyAsValue<IEnumerable<object>>(nameof(Pagination<object>.Data));
				var count = context.Object.GetPropertyAsValue<long>(nameof(Pagination<object>.Count));
				var pageSize = context.Object.GetPropertyAsValue<int>(nameof(Pagination<object>.PageSize));
				var page = context.Object.GetPropertyAsValue<int>(nameof(Pagination<object>.Page));
				var pagination = new Pagination<object>(items, count, pageSize, page);

				hateoasOutput = GeneratePaginatedHateoasOutput(pagination, context.ObjectType, context.HttpContext);
			}
			else if (context.ObjectType.GetInterfaces().Contains(typeof(IEnumerable)))
			{
				hateoasOutput = GenerateEnumerableHateoasOutput(context.Object as IEnumerable<object>, 
					context.ObjectType,
					context.HttpContext);
			}
			else
			{
				hateoasOutput = GenerateHateoasOutput(context.Object, context.ObjectType, context.HttpContext);
			}

			context.HttpContext.Response.ContentType = SupportedMediaTypes.Last();
			return context.HttpContext.Response.WriteAsync(hateoasOutput);
		}

		private static string GeneratePaginatedHateoasOutput(Pagination<object> pagination, Type sourceType,
			HttpContext context)
		{
			var itemType = sourceType.GetGenericArguments().FirstOrDefault();
			var innerResourceValues = pagination.Data
				.Select(item => WrapDataWithHateoas(itemType, item, context));

			var paginatedResource = new Pagination<Resource>(
				innerResourceValues, pagination.Count, pagination.PageSize, pagination.Page);

			Resource dataWithHateoas = WrapDataWithHateoas(sourceType, paginatedResource, context);
			return SerializeHateoasData(dataWithHateoas);
		}

		private static string GenerateEnumerableHateoasOutput(IEnumerable<object> enumerable, Type sourceType,
			HttpContext context)
		{
			var itemType = sourceType.GetGenericArguments().FirstOrDefault();
			var innerResourceValues = enumerable.Select(item => WrapDataWithHateoas(itemType, item, context));
			Resource dataWithHateoas = WrapDataWithHateoas(sourceType, innerResourceValues, context);
			return SerializeHateoasData(dataWithHateoas);
		}

		private static string GenerateHateoasOutput(object value, Type sourceType, HttpContext context)
		{
			Resource dataWithHateoas = WrapDataWithHateoas(sourceType, value, context);
			return SerializeHateoasData(dataWithHateoas);
		}

		private static string SerializeHateoasData(Resource dataWithHateoas)
		{
			var serializerOptions = new JsonSerializerOptions
			{
				IgnoreNullValues = true,
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			};
			serializerOptions.Converters.Add(new GuidConverter());
			serializerOptions.Converters.Add(new DateTimeConverter());
			return JsonSerializer.Serialize(dataWithHateoas, serializerOptions);
		}

		private static PaginationResource<Resource> WrapDataWithHateoas(Type targetResourceType,
			Pagination<Resource> value,
			HttpContext context)
		{
			var paginatedResource = new PaginationResource<Resource>(value);
			paginatedResource.ApplyHateoasLinks(targetResourceType, context);
			return paginatedResource;
		}

		private static EnumerableResource<Resource> WrapDataWithHateoas(Type targetResourceType,
			IEnumerable<Resource> value,
			HttpContext context)
		{
			var enumerableResource = new EnumerableResource<Resource>(value);
			enumerableResource.ApplyHateoasLinks(targetResourceType, context);
			return enumerableResource;
		}

		private static SingleResource WrapDataWithHateoas(Type targetResourceType, object value, HttpContext context)
		{
			var singleResource = new SingleResource(value);
			singleResource.ApplyHateoasLinks(targetResourceType, context);
			return singleResource;
		}
	}
}