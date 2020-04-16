using System.Web.Http;
using HateoasNet.Framework.DependencyInjection;
using HateoasNet.Framework.Sample.HateoasMaps;

namespace HateoasNet.Framework.Sample
{
	public static class ApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API routes
			config.MapHttpAttributeRoutes();

			// Hateoas services registrations
			config.ConfigureHateoasMap(hateoasConfig =>
				hateoasConfig.ApplyConfigurationsFromAssembly(typeof(GuildHateoas).Assembly));
		}
	}
}