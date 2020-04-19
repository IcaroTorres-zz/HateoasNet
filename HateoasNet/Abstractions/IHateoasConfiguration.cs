using System;
using System.Collections.Generic;
using System.Reflection;

namespace HateoasNet.Abstractions
{
	public interface IHateoasConfiguration
	{
		bool HasMap(Type type);
		IEnumerable<IHateoasLink> GetMappedLinks(Type sourceType, object resourceData);
		IHateoasConfiguration Map<T>(Action<IHateoasMap<T>> mapper) where T : class;
		IHateoasConfiguration ApplyConfiguration<T>(IHateoasBuilder<T> builder) where T : class;
		IHateoasConfiguration ApplyConfigurationsFromAssembly(Assembly assembly);
	}
}