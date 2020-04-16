using System.Web.Http;

namespace HateoasNet.Framework.Sample
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			ApiConfig.Register(GlobalConfiguration.Configuration);
		}
	}
}