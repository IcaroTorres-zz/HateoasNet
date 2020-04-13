using System;

namespace HateoasNet.Abstractions
{
	public interface IHateoasWriter
	{
		string Write(object value, Type objectType);
	}
}