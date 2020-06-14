using HateoasNet.Abstractions;
using HateoasNet.Framework.Sample.Models;

namespace HateoasNet.Framework.Sample.HateoasBuilders
{
    public class MemberHateoasBuilder : IHateoasSourceBuilder<Member>
    {
        public void Build(IHateoasSource<Member> resource)
        {
            resource.AddLink("get-member").HasRouteData(e => new { id = e.Id });
            resource.AddLink("update-member").HasRouteData(e => new { id = e.Id });

            resource
                .AddLink("get-guild")
                .HasRouteData(e => new { id = e.GuildId })
                .When(e => e.GuildId != null);

            resource
                .AddLink("promote-member")
                .HasRouteData(e => new { id = e.Id })
                .When(e => e.GuildId != null && !e.IsGuildMaster);

            resource
                .AddLink("demote-member")
                .HasRouteData(e => new { id = e.Id })
                .When(e => e.GuildId != null && e.IsGuildMaster);

            resource.AddLink("leave-guild")
                    .HasRouteData(e => new { id = e.Id })
                    .When(e => e.GuildId != null);
        }
    }
}
