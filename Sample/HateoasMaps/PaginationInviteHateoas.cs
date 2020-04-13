using HateoasNet.Abstractions;
using HateoasNet.Core;
using HateoasNet.Resources;
using Sample.Models;

namespace Sample.HateoasMaps
{
	public class PaginationInviteHateoas : IHateoasBuilder<Pagination<Invite>>
	{
		public void Build(HateoasMap<Pagination<Invite>> map)
		{
			map.HasLink("get-invites");
			map.HasLink("invite-member");
		}
	}
}