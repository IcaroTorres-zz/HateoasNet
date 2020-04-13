using System;
using System.Collections.Generic;
using HateoasNet.Resources;

namespace HateoasNet.Abstractions
{
	public interface IHateoasConverter
	{
		PaginationResource<Resource> ToPaginationResource(object value, Type objectType);
		EnumerableResource<Resource> ToEnumerableResource(object value, Type objectType);
		SingleResource ToSingleResource(object source, Type type);
	}
}