namespace HateoasNet.Abstractions
{
	public interface IHateoasBuilder<T> where T : class
	{
		void Build(IHateoasMap<T> map);
	}
}