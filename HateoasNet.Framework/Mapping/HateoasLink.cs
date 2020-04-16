using System;
using System.Web.Routing;
using HateoasNet.Abstractions;
using HateoasNet.Mapping;

namespace HateoasNet.Framework.Mapping
{
	public class HateoasLink<T> : AbstractHateoasLink<T> where T : class
	{
		protected internal HateoasLink(string routeName) : base(routeName)
		{
		}

		public override IHateoasLink<T> HasRouteData(Func<T, object> routeDataFunction)
		{
			if (routeDataFunction == null) throw new ArgumentNullException(nameof(routeDataFunction));

			RouteDictionaryFunction = source => new RouteValueDictionary(routeDataFunction(source));

			return this;
		}
	}
}