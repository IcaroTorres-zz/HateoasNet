using HateoasNet.Abstractions;
using HateoasNet.Factories;
using HateoasNet.Framework.Factories;
using HateoasNet.Framework.Formatting;
using HateoasNet.Framework.Sample.HateoasConfigurations;
using HateoasNet.Framework.Sample.JsonData;
using HateoasNet.Framework.Serialization;
using System;
using Unity;

namespace HateoasNet.Framework.Sample
{
    /// <summary>
    ///   Specifies the Unity configuration for the main _container.
    /// </summary>
    public static class UnityConfig
    {
        private static readonly Lazy<IUnityContainer> _container =
            new Lazy<IUnityContainer>(() =>
            {
                var container = new UnityContainer();
                RegisterTypes(container);
                return container;
            });

        /// <summary>
        ///   Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => _container.Value;

        /// <summary>
        ///   Registers the type mappings with the Unity _container.
        /// </summary>
        /// <param name="container">The unity _container to configure.</param>
        /// <remarks>
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            container
                .RegisterFactory<IHateoasContext>(f => HateoasConfig.ConfigureFromAssembly(typeof(GuildHateoasResource)))
                .RegisterFactory<IResourceLinkFactory>(f => new ResourceLinkFactory())
                .RegisterType<IHateoasSerializer, HateoasSerializer>()
                .RegisterType<IResourceFactory, ResourceFactory>()
                .RegisterType<HateoasMediaTypeFormatter>()
                .RegisterType<Seeder>();
        }
    }
}
