using HateoasNet.Abstractions;
using HateoasNet.Framework.Sample.Models;

namespace HateoasNet.Framework.Sample.HateoasConfigurations
{
    public class MemberHateoasResource : IHateoasResourceConfiguration<Member>
    {
        public void Configure(IHateoasResource<Member> resource)
        {
            resource.HasLink("get-member").HasRouteData(e => new { id = e.Id });
            resource.HasLink("update-member").HasRouteData(e => new { id = e.Id });

            resource
                .HasLink("get-guild")
                .HasRouteData(e => new { id = e.GuildId })
                .HasConditional(e => e.GuildId != null);

            resource
                .HasLink("promote-member")
                .HasRouteData(e => new { id = e.Id })
                .HasConditional(e => e.GuildId != null && !e.IsGuildMaster);

            resource
                .HasLink("demote-member")
                .HasRouteData(e => new { id = e.Id })
                .HasConditional(e => e.GuildId != null && e.IsGuildMaster);

            resource.HasLink("leave-guild")
                .HasRouteData(e => new { id = e.Id })
                .HasConditional(e => e.GuildId != null);
        }
    }
}