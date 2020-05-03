using System.Collections;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using HateoasNet.Abstractions;
using HateoasNet.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;

namespace HateoasNet.Core.Formatting
{
  /// <inheritdoc cref="OutputFormatter" />
  public class HateoasFormatter : OutputFormatter
  {
    private IHateoasSerializer _hateoasSerializer;
    private IResourceFactory _resourceFactory;

    /// <summary>
    /// 	Constructor with dependencies injected for testing purposes.
    /// </summary>
    public HateoasFormatter(IResourceFactory resourceFactory, IHateoasSerializer hateoasSerializer) : this()
    {
      _resourceFactory = resourceFactory;
      _hateoasSerializer = hateoasSerializer;
    }

    /// <summary>
    /// 	Parameterless Constructor, used when getting dependencies within <see cref="WriteResponseBodyAsync" />
		///		implementation through <see cref="HttpContext.RequestServices"/>.
    /// </summary>
    public HateoasFormatter()
    {
      SupportedMediaTypes.Add("application/json");
      SupportedMediaTypes.Add("application/json+hateoas");
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
    {
      if (context.Object is SerializableError error)
      {
        var errorOutput = JsonSerializer.Serialize(error);
        context.HttpContext.Response.ContentType = SupportedMediaTypes.First();
        await context.HttpContext.Response.WriteAsync(errorOutput);
        return;
      }

      _resourceFactory ??= context.HttpContext.RequestServices.GetRequiredService<IResourceFactory>();
      _hateoasSerializer ??= context.HttpContext.RequestServices.GetRequiredService<IHateoasSerializer>();

      Resource resource = context.Object switch
      {
        IPagination pagination => _resourceFactory.Create(pagination, context.ObjectType),
        IEnumerable enumerable => _resourceFactory.Create(enumerable, context.ObjectType),
        _ => _resourceFactory.Create(context.Object, context.ObjectType)
      };
      var formattedResponse = _hateoasSerializer.SerializeResource(resource);

      context.HttpContext.Response.ContentType = SupportedMediaTypes.Last();
      await context.HttpContext.Response.WriteAsync(formattedResponse);
    }
  }
}
