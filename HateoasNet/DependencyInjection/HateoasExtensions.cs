using System;
using HateoasNet.Abstractions;
using HateoasNet.Core;
using HateoasNet.Formatting;
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
				.AddTransient<IHateoasConverter, HateoasConverter>()
				.AddTransient<IHateoasSerializer, HateoasSerializer>()
				.AddTransient<IHateoasWriter, HateoasWriter>();
		}

		public static IMvcBuilder ConfigureHateoasMap(this IMvcBuilder builder, Action<HateoasConfiguration> config = null)
		{
			if (config != null) builder.Services.Configure(config);
			return builder.AddMvcOptions(o => o.OutputFormatters.Add(new HateoasOutputFormatter()));
		}
	}
}