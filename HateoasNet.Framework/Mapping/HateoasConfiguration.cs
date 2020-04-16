using System;
using HateoasNet.Abstractions;
using HateoasNet.Mapping;

namespace HateoasNet.Framework.Mapping
{
	public class HateoasConfiguration : AbstractHateoasConfiguration
	{
		protected internal override IHateoasMap<T> GetOrInsert<T>()
		{
			var targetType = typeof(T);

			if (!Maps.ContainsKey(targetType)) Maps.Add(targetType, new HateoasMap<T>());

			return Maps[targetType] as IHateoasMap<T>;
		}

		protected internal override IHateoasMap GetOrInsert(Type targetType)
		{
			if (Maps.ContainsKey(targetType)) return Maps[targetType];
			
			var hateoasMapType = typeof(HateoasMap<>).MakeGenericType(targetType);
			Maps.Add(targetType, Activator.CreateInstance(hateoasMapType) as IHateoasMap);

			return Maps[targetType];
		}
	}
}