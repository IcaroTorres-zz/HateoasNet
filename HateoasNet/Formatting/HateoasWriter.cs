using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using HateoasNet.Converters;
using HateoasNet.Resources;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace HateoasNet.Formatting
{
	public class HateoasWriter
	{
		private readonly OutputFormatterWriteContext _context;
		private readonly ResourceConverter _resourceConverter;

		public HateoasWriter(OutputFormatterWriteContext context, ResourceConverter resourceConverter)
		{
			_context = context;
			_resourceConverter = resourceConverter;
		}

		public string WriteHateoasOutput()
		{
			if (_context.ObjectType.IsGenericTypeDefinition &&
			    _context.ObjectType.GetGenericTypeDefinition() == typeof(Pagination<>))
				return WriteAsPagination();

			return _context.ObjectType.GetInterfaces().Contains(typeof(IEnumerable)) ? WriteAsEnumeration() : WriteAsSingle();
		}

		private string WriteAsPagination()
		{
			var originalPagination = _context.Object.ExtractPagination();
			var itemType = _context.ObjectType.GetGenericArguments().First();

			var singleResources =
				originalPagination.Data.Select(item => _resourceConverter.ToSingle(item, itemType));

			var resourcePagination = new Pagination<Resource>(singleResources,
				originalPagination.Count,
				originalPagination.PageSize,
				originalPagination.Page);

			var resource = _resourceConverter.ToPagination(resourcePagination, _context.ObjectType);
			return SerializeFormattedResource(resource);
		}

		private string WriteAsEnumeration()
		{
			var enumerable = _context.Object as IEnumerable<object>;
			var itemType = _context.ObjectType.GetGenericArguments().First();
			var singleResources = enumerable.Select(item => _resourceConverter.ToSingle(item, itemType));
			var resource = _resourceConverter.ToEnumerable(singleResources, _context.ObjectType);
			return SerializeFormattedResource(resource);
		}

		private string WriteAsSingle()
		{
			var resource = _resourceConverter.ToSingle(_context.Object, _context.ObjectType);
			return SerializeFormattedResource(resource);
		}

		private static string SerializeFormattedResource(Resource resource)
		{
			var serializerOptions = new JsonSerializerOptions
			{
				IgnoreNullValues = true,
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			};
			serializerOptions.Converters.Add(new GuidConverter());
			serializerOptions.Converters.Add(new DateTimeConverter());
			return JsonSerializer.Serialize(resource, serializerOptions);
		}
	}
}