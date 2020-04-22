using HateoasNet.Abstractions;
using HateoasNet.Framework.Sample.Models;

namespace HateoasNet.Framework.Sample.HateoasConfigurations
{
	public class InviteHateoasResource : IHateoasResourceConfiguration<Invite>
	{
		public void Configure(IHateoasResource<Invite> resource)
		{
			resource.HasLink("accept-invite")
				.HasRouteData(e => new {id = e.Id})
				.HasConditional(e => e.Status == InviteStatuses.Pending);
			
			resource.HasLink("decline-invite")
				.HasRouteData(e => new {id = e.Id})
				.HasConditional(e => e.Status == InviteStatuses.Pending);
			
			resource.HasLink("cancel-invite")
				.HasRouteData(e => new {id = e.Id})
				.HasConditional(e => e.Status == InviteStatuses.Pending);

			resource.HasLink("get-invite").HasRouteData(i => new {id = i.Id});
			resource.HasLink("get-guild").HasRouteData(i => new {id = i.GuildId});
			resource.HasLink("get-member").HasRouteData(i => new {id = i.MemberId});
		}
	}
}