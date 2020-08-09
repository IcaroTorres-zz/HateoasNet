using HateoasNet.Abstractions;
using HateoasNet.Framework.Sample.Models;

namespace HateoasNet.Framework.Sample.HateoasBuilders
{
    public class InviteHateoasBuilder : IHateoasSourceBuilder<Invite>
    {
        public void Build(IHateoasSource<Invite> source)
        {
            source.AddLink("accept-invite")
                    .HasRouteData(e => new { id = e.Id })
                    .When(e => e.Status == InviteStatuses.Pending);

            source.AddLink("decline-invite")
                    .HasRouteData(e => new { id = e.Id })
                    .When(e => e.Status == InviteStatuses.Pending);

            source.AddLink("cancel-invite")
                    .HasRouteData(e => new { id = e.Id })
                    .When(e => e.Status == InviteStatuses.Pending);

            source.AddLink("get-invite").HasRouteData(i => new { id = i.Id });
            source.AddLink("get-guild").HasRouteData(i => new { id = i.GuildId });
            source.AddLink("get-member").HasRouteData(i => new { id = i.MemberId });
        }
    }
}
