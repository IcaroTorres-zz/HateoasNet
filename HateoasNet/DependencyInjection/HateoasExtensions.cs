using HateoasNet.Abstractions;
using HateoasNet.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HateoasNet.DependencyInjection
{

    internal static class CustomHateoasExtensions
    {
        internal static IEnumerable<(TypeInfo, Type)> GetServiceTuplesFromAssemblies(this Assembly[] assemblies)
        {
            return assemblies.SelectMany(a => a.DefinedTypes
                    .Select(t => (t, t.GetInterfaces().SingleOrDefault(IsCustomHateoas)))
                    .Where(x => x.Item2 != null));
        }

        internal static bool IsCustomHateoas(Type type)
        {
            return type.IsInterface && type.Name.Contains(typeof(IHateoas<>).Name);
        }
    }
}

#if NETCOREAPP3_1
namespace HateoasNet.DependencyInjection.Core
{
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.Extensions.DependencyInjection;

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

        public static IServiceCollection RegisterAllCustomHateoas(this IServiceCollection services, Assembly[] assemblies, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            foreach (var (implementation, abstraction) in assemblies.GetServiceTuplesFromAssemblies())
                services.Add(new ServiceDescriptor(abstraction, implementation, lifetime));

            return services;
        }
    }
}
#elif NET472
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
