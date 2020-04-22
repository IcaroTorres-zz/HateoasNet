using System;
using HateoasNet.Abstractions;
using HateoasNet.Configurations;

namespace HateoasNet.Framework.DependencyInjection
{
	public static class HateoasExtensions
	{
		/// <summary>
		///   Configure Hateoas Resource mapping in .Net Framework (Full) Web Api
		/// </summary>
		/// <param name="configuration"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static IHateoasContext ConfigureHateoas(Func<IHateoasContext, IHateoasContext> configuration)
		{
			if (configuration == null) throw new ArgumentNullException(nameof(configuration));

			return configuration(new HateoasContext());
		}
	}
}