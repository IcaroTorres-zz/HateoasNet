using HateoasNet.Abstractions;
using HateoasNet.Framework.Sample.Models;
using HateoasNet.Resources;

namespace HateoasNet.Framework.Sample.HateoasConfigurations
{
	public class PaginationInviteHateoasResource : IHateoasResourceConfiguration<Pagination<Invite>>
	{
		public void Configure(IHateoasResource<Pagination<Invite>> resource)
		{
			resource.HasLink("get-invites");
			resource.HasLink("invite-member");
		}
	}
}
