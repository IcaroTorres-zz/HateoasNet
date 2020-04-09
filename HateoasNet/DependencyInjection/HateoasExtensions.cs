using System;
using HateoasNet.Core;
using HateoasNet.Formatting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace HateoasNet.DependencyInjection
{
	public static class HateoasExtensions
	{
		public static IMvcBuilder EnableHateoas(this IMvcBuilder builder, Action<HateoasNetOptions> options = null)
		{
			if (options != null) builder.Services.Configure(options);
			builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
			builder.AddMvcOptions(o => o.OutputFormatters.Add(new JsonHateoasOutputFormatter()));
			return builder;
		}
	}
}