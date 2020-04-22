using System;
using HateoasNet.Abstractions;
using HateoasNet.Configurations;
using HateoasNet.Factories;
using HateoasNet.Core.Formatting;
using HateoasNet.Core.Resources;
using HateoasNet.Core.Serialization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace HateoasNet.Core.DependencyInjection
{
	public static class HateoasExtensions
	{
		/// <summary>
		/// Configure Hateoas Resource mapping in .Net Core Web Api and register HateoasNet required services to
		/// .net core default dependency injection container 
		/// </summary>
		/// <param name="services"></param>
		/// <param name="hateoasConfiguration"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static IServiceCollection ConfigureHateoas(this IServiceCollection services,
			Func<IHateoasContext, IHateoasContext> hateoasConfiguration)
		{
			if (hateoasConfiguration == null) throw new ArgumentNullException(nameof(hateoasConfiguration));

			return services
				.AddSingleton<IActionContextAccessor, ActionContextAccessor>()
				.AddSingleton<IUrlHelperFactory, UrlHelperFactory>()
				.AddTransient(x => hateoasConfiguration(new HateoasContext()))
				.AddTransient<IUrlBuilder, UrlBuilder>()
				.AddTransient<IHttpMethodFinder, HttpMethodFinder>()
				.AddTransient<IResourceLinkFactory, ResourceLinkFactory>()
				.AddTransient<IResourceFactory, ResourceFactory>()
				.AddTransient<IHateoasSerializer, HateoasSerializer>();
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