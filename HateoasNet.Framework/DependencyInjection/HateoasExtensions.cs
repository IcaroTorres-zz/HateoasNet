using System;
using HateoasNet.Abstractions;
using HateoasNet.Framework.Mapping;

namespace HateoasNet.Framework.DependencyInjection
{
	public static class HateoasExtensions
	{
		/// <summary>
		///   Configure Hateoas Resource mapping in .Net Framework (Full) Web Api
		/// </summary>
		/// <param name="mapConfiguration"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static IHateoasConfiguration ConfigureHateoasMap(
			Func<IHateoasConfiguration, IHateoasConfiguration> mapConfiguration)
		{
			if (mapConfiguration == null) throw new ArgumentNullException(nameof(mapConfiguration));

			var hateoasConfiguration = new HateoasConfiguration();
			mapConfiguration(hateoasConfiguration);

			return hateoasConfiguration;
		}
	}
}