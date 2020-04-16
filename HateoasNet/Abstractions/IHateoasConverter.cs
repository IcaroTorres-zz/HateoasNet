using System;
using HateoasNet.Resources;

namespace HateoasNet.Abstractions
{
	public interface IHateoasConverter
	{
		PaginationResource<Resource> ToPaginationResource(object source, Type objectType);
		EnumerableResource<Resource> ToEnumerableResource(object source, Type objectType);
		SingleResource ToSingleResource(object source, Type type);
	}
}