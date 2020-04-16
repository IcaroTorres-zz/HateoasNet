using System;
using HateoasNet.Abstractions;
using HateoasNet.Core.Formatting;
using HateoasNet.Core.Mapping;
using HateoasNet.Core.Resources;
using HateoasNet.Core.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace HateoasNet.Core.DependencyInjection
{
	public static class HateoasExtensions
	{
		/// <summary>
		/// Configure Hateoas Resource mapping in .Net Core Web Api and register HateoasNet required services to .net core default dependency injection container 
		/// </summary>
		/// <param name="services"></param>
		/// <param name="mapConfiguration"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static IServiceCollection ConfigureHateoasMap(this IServiceCollection services,
			Action<IHateoasConfiguration> mapConfiguration)
		{
			if (mapConfiguration == null) throw new ArgumentNullException(nameof(mapConfiguration));

			var hateoasConfiguration = new HateoasConfiguration();
			mapConfiguration(hateoasConfiguration);

			return services.AddSingleton<IActionContextAccessor, ActionContextAccessor>()
				.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
				.AddScoped<IHateoasConfiguration, HateoasConfiguration>(x => hateoasConfiguration)
				.AddScoped<IResourceFactory, ResourceFactory>()
				.AddScoped<IHateoasSerializer, HateoasSerializer>();
		}

		/// <summary>
		/// Add HateoasFormatter to MvcBuilder OutputFormatters  
		/// </summary>
		/// <param name="builder"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static IMvcBuilder AddHateoasFormatter(this IMvcBuilder builder)
		{
			return builder.AddMvcOptions(o => o.OutputFormatters.Add(new HateoasFormatter()));
		}
	}
}