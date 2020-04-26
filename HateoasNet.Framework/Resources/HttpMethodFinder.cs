using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using HateoasNet.Abstractions;

namespace HateoasNet.Framework.Resources
{
	public class HttpMethodFinder : IHttpMethodFinder
	{
		private readonly IEnumerable<HttpActionDescriptor> _actionDescriptors;

		public HttpMethodFinder(IEnumerable<HttpActionDescriptor> actionDescriptors)
		{
			_actionDescriptors = actionDescriptors;
		}

		public string Find(string routeName)
		{
			return _actionDescriptors
			       .SingleOrDefault(x => x.GetRouteAttribute().Name == routeName)
			       .GetMethod().Method;
		}
	}
}