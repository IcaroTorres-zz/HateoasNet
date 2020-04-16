using System;
using System.Collections;
using System.Collections.Generic;
using HateoasNet.Resources;

namespace HateoasNet.Abstractions
{
	public interface IResourceFactory
	{
		Resource Create(object source, Type objectType);
		Resource Create(IEnumerable source, Type objectType);
		Resource Create(IPagination source, Type objectType);
		Resource ApplyLinks(Resource source, Type type);
		Resource ApplyLinks(IEnumerable<Resource> source, Type type);
		Resource ApplyLinks(Pagination<Resource> source, Type type);
	}
}