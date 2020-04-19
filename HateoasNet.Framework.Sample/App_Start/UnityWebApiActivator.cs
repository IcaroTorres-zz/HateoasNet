using System.Web.Http;
using HateoasNet.Framework.Sample;
using Unity.AspNet.WebApi;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(UnityWebApiActivator), nameof(UnityWebApiActivator.Start))]
[assembly: ApplicationShutdownMethod(typeof(UnityWebApiActivator), nameof(UnityWebApiActivator.Shutdown))]

namespace HateoasNet.Framework.Sample
{
    /// <summary>
    ///   Provides the bootstrapping for integrating Unity with WebApi when it is hosted in ASP.NET.
    /// </summary>
    public static class UnityWebApiActivator
	{
        /// <summary>
        ///   Integrates Unity when the application starts.
        /// </summary>
        public static void Start()
		{
			var resolver = new UnityHierarchicalDependencyResolver(UnityConfig.Container);

			GlobalConfiguration.Configuration.DependencyResolver = resolver;
		}

        /// <summary>
        ///   Disposes the Unity container when the application is shut down.
        /// </summary>
        public static void Shutdown()
		{
			UnityConfig.Container.Dispose();
		}
	}
}