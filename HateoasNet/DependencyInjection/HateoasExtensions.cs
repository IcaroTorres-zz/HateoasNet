using System;
using HateoasNet.Abstractions;
using HateoasNet.Formatting;
using HateoasNet.Mapping;
#if !NET472
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace HateoasNet.DependencyInjection
{
	public static class HateoasExtensions
	{
		/// <summary>
		/// Register HateoasNet required services to .net core default dependency injection container 
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddHateoasServices(this IServiceCollection services)
		{
			return services.AddSingleton<IActionContextAccessor, ActionContextAccessor>()
				.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
				.AddTransient<IHateoasConverter, HateoasConverter>()
				.AddTransient<IHateoasSerializer, HateoasSerializer>()
				.AddTransient<IHateoasWriter, HateoasWriter>();
		}

		/// <summary>
		/// Configure Hateoas Resource mapping in .Net Core Web Api
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="mapConfiguration"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static IMvcBuilder ConfigureHateoasMap(this IMvcBuilder builder,
			Action<IHateoasConfiguration> mapConfiguration)
		{
			if (mapConfiguration == null) throw new ArgumentNullException(nameof(mapConfiguration));

			var hateoasConfiguration = new HateoasConfiguration();
			mapConfiguration(hateoasConfiguration);
			builder.Services.AddTransient<IHateoasConfiguration, HateoasConfiguration>(x => hateoasConfiguration);
			return builder.AddMvcOptions(o => o.OutputFormatters.Add(new HateoasOutputFormatter()));
		}
	}
}
#else
using System.Web.Http;

namespace HateoasNet.DependencyInjection
{
	public static class HateoasExtensions
	{
		/// <summary>
		/// Configure Hateoas Resource mapping in .Net Framework (Full) Web Api
		/// </summary>
		/// <param name="configuration"></param>
		/// <param name="mapConfiguration"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static HttpConfiguration ConfigureHateoasMap(this HttpConfiguration configuration,
			Action<IHateoasConfiguration> mapConfiguration)
		{
			if (mapConfiguration == null) throw new ArgumentNullException(nameof(mapConfiguration));

			var hateoasConfiguration = new HateoasConfiguration();
			mapConfiguration(hateoasConfiguration);
			var hateoasWriter = new HateoasWriter(new HateoasConverter(), new HateoasSerializer());

			configuration.Formatters.Add(new HateoasMediaTypeFormatter(hateoasConfiguration, hateoasWriter));

			return configuration;
		}
	}
}
#endif