using HateoasNet.Abstractions;
using HateoasNet.Framework.Sample.Models;
using HateoasNet.Resources;

namespace HateoasNet.Framework.Sample.HateoasMaps
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