using HateoasNet.Abstractions;
using HateoasNet.Framework.Sample.JsonData;
using HateoasNet.Framework.Sample.HateoasBuilders;
using System;
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
				.RegisterFactory<IHateoasContext>(f => HateoasConfig.ConfigureFromAssembly(typeof(GuildHateoasBuilder)))
				.RegisterFactory<IHateoas>(f => new Hateoas(f.Resolve<IHateoasContext>()))
				.RegisterType<Seeder>();
		}
	}
}
