using System.Collections.Generic;

namespace HateoasNet.Abstractions
{
	public interface IHateoasMap
	{
		IEnumerable<IHateoasLink> GetLinks();
	}
}