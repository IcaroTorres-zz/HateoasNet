using HateoasNet.Abstractions;
using HateoasNet.Core;
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
			map.HasLink("get-invite").HasRouteData(i => new {id = i.Id});
			map.HasLink("get-guild").HasRouteData(i => new {id = i.GuildId});
			map.HasLink("get-member").HasRouteData(i => new {id = i.MemberId});
		}
	}
}