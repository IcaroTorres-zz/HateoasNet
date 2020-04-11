namespace HateoasNet.Core.Abstractions
{
	public interface IHateoasBuilder<T> where T : class
	{
		void Build(HateoasMap<T> map);
	}
}