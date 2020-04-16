using System;
using System.Web.Http;
using System.Web.Http.Routing;
using HateoasNet.Abstractions;
using HateoasNet.Framework.Serialization;
using HateoasNet.Framework.Formatting;
using HateoasNet.Framework.Mapping;
using HateoasNet.Framework.Resources;

namespace HateoasNet.Framework.DependencyInjection
{
	public static class HateoasExtensions
	{
		/// <summary>
		/// Configure Hateoas Resource mapping in .Net Framework (Full) Web Api
		/// </summary>
		/// <param name="config"></param>
		/// <param name="mapConfiguration"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static HttpConfiguration ConfigureHateoasMap(this HttpConfiguration config,
			Action<IHateoasConfiguration> mapConfiguration)
		{
			if (mapConfiguration == null) throw new ArgumentNullException(nameof(mapConfiguration));

			IHateoasConfiguration hateoasConfiguration = new HateoasConfiguration();
			mapConfiguration(hateoasConfiguration);

			var urlHelper = new UrlHelper();
			IResourceFactory resourceFactory = new ResourceFactory(hateoasConfiguration, urlHelper);
			IHateoasSerializer hateoasSerializer = new HateoasSerializer();

			config.Services.Add(typeof(IHateoasConfiguration), hateoasConfiguration);
			config.Services.Add(typeof(UrlHelper), urlHelper);
			config.Services.Add(typeof(IResourceFactory), resourceFactory);
			config.Services.Add(typeof(IHateoasSerializer), hateoasSerializer);

			hateoasConfiguration = config.Services.GetService(typeof(IHateoasConfiguration)) as IHateoasConfiguration;
			resourceFactory = config.Services.GetService(typeof(IResourceFactory)) as IResourceFactory;
			hateoasSerializer = config.Services.GetService(typeof(IHateoasSerializer)) as IHateoasSerializer;

			config.Formatters.Add(new HateoasMediaTypeFormatter(hateoasConfiguration, resourceFactory, hateoasSerializer));

			return config;
		}
	}
}