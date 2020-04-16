using System;
using HateoasNet.Abstractions;
using HateoasNet.Mapping;

namespace HateoasNet.Framework.Mapping
{
	public class HateoasMap<T> : AbstractHateoasMap<T> where T : class
	{
		public override IHateoasLink<T> HasLink(string routeName)
		{
			if (routeName == null) throw new ArgumentNullException(nameof(routeName));
			
			HateoasLinks[routeName] = new HateoasLink<T>(routeName);

			return HateoasLinks[routeName] as IHateoasLink<T>;
		}
	}
}