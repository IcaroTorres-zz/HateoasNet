using HateoasNet.Abstractions;
using HateoasNet.Core.Sample.Models;
using HateoasNet.Resources;

namespace HateoasNet.Core.Sample.HateoasMaps
{
	public class PaginationInviteHateoas : IHateoasBuilder<Pagination<Invite>>
	{
		public void Build(IHateoasMap<Pagination<Invite>> map)
		{
			map.HasLink("get-invites");
			map.HasLink("invite-member");
		}
	}
}