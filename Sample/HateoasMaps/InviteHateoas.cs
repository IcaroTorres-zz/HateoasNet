using HateoasNet.Core;
using HateoasNet.Core.Abstractions;
using Sample.Models;

namespace Sample.HateoasMaps
{
	public class InviteHateoas : IHateoasBuilder<Invite>
	{
		public void Build(HateoasMap<Invite> map)
		{
			// map type with links for single objects using full shorthand
			map.HasLink("accept-invite", e => new {id = e.Id}, e => e.Status == InviteStatuses.Pending);
			map.HasLink("decline-invite", e => new {id = e.Id}, e => e.Status == InviteStatuses.Pending);
			map.HasLink("cancel-invite", e => new {id = e.Id}, e => e.Status == InviteStatuses.Pending);

			// map type with links for single objects using inline routeData function
			map.HasLink("get-invite").WithData(i => new {id = i.Id});
			map.HasLink("get-guild").WithData(i => new {id = i.GuildId});
			map.HasLink("get-member").WithData(i => new {id = i.MemberId});
		}
	}
}