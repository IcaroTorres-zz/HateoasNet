using System.Collections.Generic;

namespace HateoasNet.Core.Abstractions
{
	public interface IHateoasMap
	{
		IReadOnlyList<IHateoasLink> GetLinks();
	}
}