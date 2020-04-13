using HateoasNet.Resources;

namespace HateoasNet.Abstractions
{
	public interface IHateoasSerializer
	{
		string SerializeResource(Resource resource);
	}
}