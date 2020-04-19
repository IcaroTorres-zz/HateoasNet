using HateoasNet.Resources;

namespace HateoasNet.Abstractions
{
	public interface IResourceLinkFactory
	{
		ResourceLink Create(Resource resource, IHateoasLink hateoasLink);
	}
}