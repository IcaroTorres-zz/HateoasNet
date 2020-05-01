using System;
using HateoasNet.Abstractions;
using HateoasNet.Framework.DependencyInjection;

namespace HateoasNet.Framework.Sample
{
	public static class HateoasConfig
	{
		public static IHateoasContext ConfigureFromAssembly(Type containedInAssembly)
		{
			return HateoasExtensions.ConfigureHateoas(context => context.ConfigureFromAssembly(containedInAssembly.Assembly));
		}
	}
}
