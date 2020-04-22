using System.Web.Http;
using HateoasNet.Framework.Formatting;

namespace HateoasNet.Framework.Sample
{
	public static class FormattersConfig
	{
		public static void RegisterFormatters(HttpConfiguration config)
		{
			// set hateoas formatter using configured IoC container
			config.Formatters.Add(
				config.DependencyResolver.GetService(typeof(HateoasMediaTypeFormatter)) as HateoasMediaTypeFormatter);
		}
	}
}