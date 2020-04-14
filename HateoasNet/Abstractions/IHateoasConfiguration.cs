using System;
using System.Collections.Generic;
using System.Reflection;
using HateoasNet.Mapping;

namespace HateoasNet.Abstractions
{
	public interface IHateoasConfiguration
	{
		IEnumerable<IHateoasLink> GetMappedLinks(Type sourceType, object resourceData);
		IHateoasConfiguration Map<T>(Action<HateoasMap<T>> mapper) where T : class;
		IHateoasConfiguration ApplyConfiguration<T>(IHateoasBuilder<T> builder) where T : class;
		IHateoasConfiguration ApplyConfigurationsFromAssembly(Assembly assembly);
	}
}