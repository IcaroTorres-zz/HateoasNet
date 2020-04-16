using System.Collections.Generic;
using HateoasNet.Abstractions;

namespace HateoasNet.Mapping
{
	public abstract class AbstractHateoasMap<T> : IHateoasMap<T> where T : class
	{
		protected readonly Dictionary<string, IHateoasLink> HateoasLinks = new Dictionary<string, IHateoasLink>();

		public virtual IEnumerable<IHateoasLink> GetLinks() => HateoasLinks.Values;

		public abstract IHateoasLink<T> HasLink(string routeName);
	}
}