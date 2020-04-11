using System;
using HateoasNet.Core;
using HateoasNet.Formatting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace HateoasNet.DependencyInjection
{
	public static class HateoasExtensions
	{
		public static IMvcBuilder ConfigureHateoasMap(this IMvcBuilder builder, Action<HateoasConfiguration> config = null)
		{
			if (config != null) builder.Services.Configure(config);
			builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
			builder.AddMvcOptions(o => o.OutputFormatters.Add(new HateoasOutputFormatter()));
			return builder;
		}
	}
}