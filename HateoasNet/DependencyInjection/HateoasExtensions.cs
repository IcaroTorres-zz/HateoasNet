using System;
using HateoasNet.Abstractions;
using HateoasNet.Formatting;
using HateoasNet.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace HateoasNet.DependencyInjection
{
	public static class HateoasExtensions
	{
		public static IServiceCollection AddHateoasServices(this IServiceCollection services)
		{
			return services.AddSingleton<IActionContextAccessor, ActionContextAccessor>()
				.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
				.AddTransient<IHateoasConfiguration, HateoasConfiguration>()
				.AddTransient<IHateoasConverter, HateoasConverter>()
				.AddTransient<IHateoasSerializer, HateoasSerializer>()
				.AddTransient<IHateoasWriter, HateoasWriter>();
		}

		public static IMvcBuilder ConfigureHateoasMap(this IMvcBuilder builder,
			Action<IHateoasConfiguration> hateoasConfiguration)
		{
			if (hateoasConfiguration == null) throw new ArgumentNullException(nameof(hateoasConfiguration));

			builder.Services.Configure(hateoasConfiguration);
			return builder.AddMvcOptions(o => o.OutputFormatters.Add(new HateoasOutputFormatter()));
		}
	}
}