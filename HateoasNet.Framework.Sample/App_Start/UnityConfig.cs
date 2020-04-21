using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Routing;
using HateoasNet.Abstractions;
using HateoasNet.Configurations;
using HateoasNet.Factories;
using HateoasNet.Framework.Formatting;
using HateoasNet.Framework.Resources;
using HateoasNet.Framework.Sample.HateoasConfigurations;
using HateoasNet.Framework.Sample.JsonData;
using HateoasNet.Framework.Serialization;
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
			// Hateoas map configuration
			var hateoasConfiguration = HateoasConfig.MapFromAssembly(new HateoasContext(), typeof(GuildHateoasResource));

			container
				.RegisterFactory<IHateoasContext>(f => hateoasConfiguration)
				.RegisterType<Seeder>()
				.RegisterFactory<IHttpMethodFinder>(f => new HttpMethodFinder(GetActionDescriptors()))
				.RegisterFactory<IUrlBuilder>(f => new UrlBuilder(GetActionDescriptors()))
				.RegisterType<IHateoasSerializer, HateoasSerializer>()
				.RegisterType<IResourceLinkFactory, ResourceLinkFactory>()
				.RegisterType<IResourceFactory, ResourceFactory>()
				.RegisterType<HateoasMediaTypeFormatter>();
		}

		private static IEnumerable<HttpActionDescriptor> GetActionDescriptors() =>
			RouteTable.Routes
				.OfType<Route>()
				.Select(route => route.DataTokens.Values.OfType<HttpActionDescriptor[]>().FirstOrDefault()?.First())
				.Where(x => x != null);
	}
}