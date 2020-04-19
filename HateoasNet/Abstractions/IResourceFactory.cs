using System;
using System.Collections;
using System.Collections.Generic;
using HateoasNet.Resources;

namespace HateoasNet.Abstractions
{
	public interface IResourceFactory
	{
		Resource Create(object source, Type type);
		Resource Create(IEnumerable source, Type type);
		Resource Create(IPagination source, Type type);
		Resource ApplyLinks(Resource source, Type type);
		Resource ApplyLinks(IEnumerable<Resource> source, Type type);
		Resource ApplyLinks(Pagination<Resource> source, Type type);
	}
}