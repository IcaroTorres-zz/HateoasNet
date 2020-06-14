#if NETCOREAPP3_1
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using HateoasNet.Abstractions;
using System;
using HateoasNet.Infrastructure;
using HateoasNet;

namespace HateoasNet.DependencyInjection.Core
{
    public static class HateoasExtensions
	{
		/// <summary>
		///   Configure Hateoas Source mapping in .Net Core Web Api and register required services to
		///   .net core default dependency injection container
		/// </summary>
		/// <param name="services">The <see cref="IServiceCollection"/> being configured on Ioc container.</param>
		/// <param name="hateoasOptions">Function callback to configure <see cref="IHateoasContext"/> options.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static IServiceCollection ConfigureHateoas(this IServiceCollection services, Func<IHateoasContext, IHateoasContext> hateoasOptions)
		{
			if (hateoasOptions == null) throw new ArgumentNullException(nameof(hateoasOptions));

			return services
				   .AddSingleton<IActionContextAccessor, ActionContextAccessor>()
				   .AddSingleton<IUrlHelperFactory, UrlHelperFactory>()
				   .AddScoped(x => hateoasOptions(new HateoasContext()))
				   .AddScoped(x => x.GetRequiredService<IUrlHelperFactory>().GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext))
				   .AddScoped<IHateoas, Hateoas>();
		}
	}
}
#elif NET472
using System;
using HateoasNet.Abstractions;
using HateoasNet.Infrastructure;

namespace HateoasNet.DependencyInjection.Framework
{
    public static class HateoasExtensions
	{
        /// <summary>
        ///   Configure Hateoas Resource mapping in .Net Framework (Full) Web Api
        /// </summary>
        /// <param name="hateoasOptions"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IHateoasContext ConfigureHateoas(Func<IHateoasContext, IHateoasContext> hateoasOptions)
		{
			if (hateoasOptions == null) throw new ArgumentNullException(nameof(hateoasOptions));

			return hateoasOptions(new HateoasContext());
		}
	}
}
#endif
