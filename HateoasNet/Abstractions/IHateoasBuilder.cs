namespace HateoasNet.Abstractions
{
	public interface IHateoasBuilder<in T> where T : class
	{
		void Build(IHateoasMap<T> map);
	}
}