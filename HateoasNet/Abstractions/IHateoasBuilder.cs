using HateoasNet.Core;

namespace HateoasNet.Abstractions
{
	public interface IHateoasBuilder<T> where T : class
	{
		void Build(HateoasMap<T> map);
	}
}