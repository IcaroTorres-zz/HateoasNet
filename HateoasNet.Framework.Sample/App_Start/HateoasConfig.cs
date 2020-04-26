using System;
using HateoasNet.Abstractions;
using HateoasNet.Configurations;

namespace HateoasNet.Framework.Sample
{
	public static class HateoasConfig
	{
		public static IHateoasContext ConfigureFromAssembly(Type containedInAssembly)
		{
			return new HateoasContext().ConfigureFromAssembly(containedInAssembly.Assembly);
		}
	}
}