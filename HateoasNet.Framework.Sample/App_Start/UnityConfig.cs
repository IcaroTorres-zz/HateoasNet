using System;
using HateoasNet.Abstractions;
using HateoasNet.Factories;
using HateoasNet.Framework.Factories;
using HateoasNet.Framework.Formatting;
using HateoasNet.Framework.Sample.HateoasConfigurations;
using HateoasNet.Framework.Sample.JsonData;
using HateoasNet.Framework.Serialization;
using Unity;

namespace HateoasNet.Framework.Sample
{
    /// <summary>
    ///   Specifies the Unity configuration for the main Container.
    /// </summary>
    public static class UnityConfig
	{
		private static readonly Lazy<IUnityContainer> Container =
			new Lazy<IUnityContainer>(() =>
			{
				var container = new UnityContainer();
				RegisterTypes(container);
				return container;
			});

        /// <summary>
        ///   Configured Unity GetContainer.
        /// </summary>
        public static IUnityContainer GetContainer()
		{
			return Container.Value;
		}

        /// <summary>
        ///   Registers the type mappings with the Unity Container.
        /// </summary>
        /// <param name="container">The unity Container to configure.</param>
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
