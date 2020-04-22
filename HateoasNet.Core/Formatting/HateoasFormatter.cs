using System.Collections;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using HateoasNet.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;

namespace HateoasNet.Core.Formatting
{
	public class HateoasFormatter : OutputFormatter
	{
		private IResourceFactory _resourceFactory;
		private IHateoasSerializer _hateoasSerializer;
		
		public HateoasFormatter(IResourceFactory resourceFactory, IHateoasSerializer hateoasSerializer)
		{
			_resourceFactory = resourceFactory;
			_hateoasSerializer = hateoasSerializer;
			
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

			_resourceFactory ??= context.HttpContext.RequestServices.GetRequiredService<IResourceFactory>();
			_hateoasSerializer ??= context.HttpContext.RequestServices.GetRequiredService<IHateoasSerializer>();
			
			var resource = context.Object switch
			{
				IPagination pagination => _resourceFactory.Create(pagination, context.ObjectType),
				IEnumerable enumerable => _resourceFactory.Create(enumerable, context.ObjectType),
				_ => _resourceFactory.Create(context.Object, context.ObjectType)
			};
			var formattedResponse = _hateoasSerializer.SerializeResource(resource);

			context.HttpContext.Response.ContentType = SupportedMediaTypes.Last();
			return context.HttpContext.Response.WriteAsync(formattedResponse);
		}
	}
}