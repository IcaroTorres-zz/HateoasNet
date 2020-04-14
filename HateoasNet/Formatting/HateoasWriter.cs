using System;
using System.Collections;
using System.Linq;
using HateoasNet.Abstractions;
using HateoasNet.Resources;

namespace HateoasNet.Formatting
{
	public class HateoasWriter : IHateoasWriter
	{
		private readonly IHateoasConverter _hateoasConverter;
		private readonly IHateoasSerializer _hateoasSerializer;

		public HateoasWriter(IHateoasConverter hateoasConverter, IHateoasSerializer hateoasSerializer)
		{
			_hateoasConverter = hateoasConverter;
			_hateoasSerializer = hateoasSerializer;
		}

		public string Write(object value, Type objectType)
		{
			Resource hateoasResource;
			
			if (objectType.GetGenericTypeDefinition() == typeof(Pagination<>))
			{
				hateoasResource = _hateoasConverter.ToPaginationResource(value, objectType);
			}
			else if (objectType.GetInterfaces().Contains(typeof(IEnumerable)))
			{
				hateoasResource = _hateoasConverter.ToEnumerableResource(value, objectType);
			}
			else hateoasResource = _hateoasConverter.ToSingleResource(value, objectType);

			return _hateoasSerializer.SerializeResource(hateoasResource);
		}
	}
}