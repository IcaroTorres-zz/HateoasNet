using System.Web;
using System.Web.Http;

namespace HateoasNet.Framework.Sample
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(config =>
            {
                RouteConfig.RegisterRoutes(config);
            });
        }
    }
}
