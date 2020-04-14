using System;
using System.Collections.Generic;

namespace HateoasNet.Abstractions
{
	public interface IHateoasMap
	{
		IEnumerable<IHateoasLink> GetLinks();
	}

	public interface IHateoasMap<out T> : IHateoasMap where T : class
	{
		IHateoasLink<T> HasLink(string routeName);

		IHateoasLink<T> HasLink(string routeName, Func<T, object> routeDataFunction,
			Func<T, bool> predicateFunction = null);
	}
}