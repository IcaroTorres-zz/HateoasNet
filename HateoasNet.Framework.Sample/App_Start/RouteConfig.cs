using System.Web.Http;

namespace HateoasNet.Framework.Sample
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(HttpConfiguration config)
        {
            // register routes
            config.MapHttpAttributeRoutes();
        }
    }
}
