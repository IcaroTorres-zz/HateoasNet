using HateoasNet.Abstractions;
using Sample.Models;

namespace Sample.HateoasMaps
{
	public class InviteHateoas : IHateoasBuilder<Invite>
	{
		public void Build(IHateoasMap<Invite> map)
		{
			map.HasLink("accept-invite")
				.HasRouteData(e => new {id = e.Id})
				.HasConditional(e => e.Status == InviteStatuses.Pending);
			
			map.HasLink("decline-invite")
				.HasRouteData(e => new {id = e.Id})
				.HasConditional(e => e.Status == InviteStatuses.Pending);
			
			map.HasLink("cancel-invite")
				.HasRouteData(e => new {id = e.Id})
				.HasConditional(e => e.Status == InviteStatuses.Pending);

			map.HasLink("get-invite").HasRouteData(i => new {id = i.Id});
			map.HasLink("get-guild").HasRouteData(i => new {id = i.GuildId});
			map.HasLink("get-member").HasRouteData(i => new {id = i.MemberId});
		}
	}
}