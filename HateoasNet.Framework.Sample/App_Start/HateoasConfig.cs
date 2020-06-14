using System;
using HateoasNet.Abstractions;
using HateoasNet.DependencyInjection.Framework;

namespace HateoasNet.Framework.Sample
{
	public static class HateoasConfig
	{
		public static IHateoasContext ConfigureFromAssembly(Type type)
		{
			return HateoasExtensions.ConfigureHateoas(context => context.ConfigureFromAssembly(type.Assembly));
		}
	}
}
