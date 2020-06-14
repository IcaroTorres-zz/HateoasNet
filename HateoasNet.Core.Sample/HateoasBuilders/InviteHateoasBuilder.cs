using HateoasNet.Abstractions;
using HateoasNet.Core.Sample.Models;

namespace HateoasNet.Core.Sample.HateoasBuilders
{
    public class InviteHateoasBuilder : IHateoasSourceBuilder<Invite>
    {
        public void Build(IHateoasSource<Invite> resource)
        {
            resource.AddLink("accept-invite")
                    .HasRouteData(e => new { id = e.Id })
                    .When(e => e.Status == InviteStatuses.Pending);

            resource.AddLink("decline-invite")
                    .HasRouteData(e => new { id = e.Id })
                    .When(e => e.Status == InviteStatuses.Pending);

            resource.AddLink("cancel-invite")
                    .HasRouteData(e => new { id = e.Id })
                    .When(e => e.Status == InviteStatuses.Pending);

            resource.AddLink("get-invite").HasRouteData(i => new { id = i.Id });
            resource.AddLink("get-guild").HasRouteData(i => new { id = i.GuildId });
            resource.AddLink("get-member").HasRouteData(i => new { id = i.MemberId });
        }
    }
}
