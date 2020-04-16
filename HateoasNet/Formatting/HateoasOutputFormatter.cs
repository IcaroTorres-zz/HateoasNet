#if !NET472
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using HateoasNet.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;

namespace HateoasNet.Formatting
{
	public class HateoasOutputFormatter : OutputFormatter
	{
		public HateoasOutputFormatter()
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

			var hateoasWriter = context.HttpContext.RequestServices.GetRequiredService<IHateoasWriter>();
			var formattedResponse = hateoasWriter.Write(context.Object, context.ObjectType);

			context.HttpContext.Response.ContentType = SupportedMediaTypes.Last();
			return context.HttpContext.Response.WriteAsync(formattedResponse);
		}
	}
}
#endif