using System.Collections.Generic;

namespace HateoasNet.Abstractions
{
	public interface IHateoasMap
	{
		IEnumerable<IHateoasLink> GetLinks();
	}

	public interface IHateoasMap<T> : IHateoasMap where T : class
	{
		IHateoasLink<T> HasLink(string routeName);
	}
}