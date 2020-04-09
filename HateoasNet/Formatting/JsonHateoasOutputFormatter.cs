using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace HateoasNet.Formatting
{
	public class JsonHateoasOutputFormatter : OutputFormatter
	{
		public JsonHateoasOutputFormatter()
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

			var resourceConverter = new ResourceConverter(context.HttpContext);
			var hateoasWriter = new HateoasWriter(context, resourceConverter);
			var hateoasStringOutput = hateoasWriter.WriteHateoasOutput();

			context.HttpContext.Response.ContentType = SupportedMediaTypes.Last();
			return context.HttpContext.Response.WriteAsync(hateoasStringOutput);
		}
	}
}