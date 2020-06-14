using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace HateoasNet.Tests.TestHelpers
{
	public class HateoasCoreDataAttribute : DataAttribute
	{
		public override IEnumerable<object[]> GetData(MethodInfo testMethod)
		{
			var data = new HateoasSample();
			var routeName = nameof(data.DocumentNumber);
			object routeParam = data.Id;
			var url = $"http://dummy.test/api/dummy/{routeName}/{routeParam}";
			var method = "GET";

			yield return new object[] { data, routeName, url, method };

			routeName = nameof(data.ZipCode);
			url = $"http://dummy.test/api/dummy/{routeName}";
			method = "POST";

			yield return new object[] { data, routeName, url, method };

			routeParam = data.ProcessNumber;
			routeName = nameof(data.ProcessNumber);
			url = $"http://dummy.test/api/dummy/{routeName}/{routeParam}";
			method = "PATCH";

			yield return new object[] { data, routeName, url, method };

			routeName = nameof(data.Email);
			routeParam = data.Email;
			url = $"http://dummy.test/api/dummy/{routeName}/{routeParam}";
			method = "DELETE";

			yield return new object[] { data, routeName, url, method };

			routeName = nameof(data.FileName);
			routeParam = data.FileName;
			url = $"http://dummy.test/api/dummy/{routeName}/{routeParam}";
			method = "PUT";

			yield return new object[] { data, routeName, url, method };

			var routeData2 = new
			{
				id = Guid.NewGuid(),
				label = "test",
				page = 21,
				pageSize = 10
			};
			routeName = "test6";
			var queryString = $"?pageSize={routeData2.pageSize}&page={routeData2.page}&label={routeData2.label}";
			url = $"http://dummy.test/api/dummy/{routeName}{queryString}";
			method = "GET";

			yield return new object[] { routeData2, routeName, url, method };
		}
	}
}
