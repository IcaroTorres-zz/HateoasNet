using HateoasNet.Framework.Formatting;
using System.Web.Http;

namespace HateoasNet.Framework.Sample
{
    public static class FormattersConfig
    {
        public static void RegisterFormatters(HttpConfiguration config)
        {
            // set hateoas formatter using configured IoC container
            var hateoasFormatter =
                config.DependencyResolver.GetService(typeof(HateoasMediaTypeFormatter)) as HateoasMediaTypeFormatter;

            config.Formatters.Add(hateoasFormatter);
        }
    }
}
