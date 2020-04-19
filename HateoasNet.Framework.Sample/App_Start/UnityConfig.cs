using System;
using HateoasNet.Abstractions;
using HateoasNet.Framework.Formatting;
using HateoasNet.Framework.Mapping;
using HateoasNet.Framework.Resources;
using HateoasNet.Framework.Sample.HateoasMaps;
using HateoasNet.Framework.Sample.JsonData;
using HateoasNet.Framework.Serialization;
using Unity;

namespace HateoasNet.Framework.Sample
{
	/// <summary>
	///   Specifies the Unity configuration for the main container.
	/// </summary>
	public static class UnityConfig
	{
		private static readonly Lazy<IUnityContainer> container =
			new Lazy<IUnityContainer>(() =>
			{
				var container = new UnityContainer();
				RegisterTypes(container);
				return container;
			});

		/// <summary>
		///   Configured Unity Container.
		/// </summary>
		public static IUnityContainer Container => container.Value;

		/// <summary>
		///   Registers the type mappings with the Unity container.
		/// </summary>
		/// <param name="container">The unity container to configure.</param>
		/// <remarks>
		/// </remarks>
		public static void RegisterTypes(IUnityContainer container)
		{
			// Hateoas map configuration
			var hateoasConfiguration = HateoasConfig.MapFromAssembly(new HateoasConfiguration(), typeof(GuildHateoas));

			container
				.RegisterFactory<IHateoasConfiguration>(f => hateoasConfiguration)
				.RegisterType<Seeder>()
				.RegisterType<IHateoasSerializer, HateoasSerializer>()
				.RegisterType<IResourceLinkFactory, ResourceLinkFactory>()
				.RegisterType<IResourceFactory, ResourceFactory>()
				.RegisterType<HateoasMediaTypeFormatter>();
		}
	}
}