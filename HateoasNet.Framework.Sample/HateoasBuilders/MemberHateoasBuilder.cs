using HateoasNet.Abstractions;
using HateoasNet.Framework.Sample.Models;

namespace HateoasNet.Framework.Sample.HateoasBuilders
{
    public class MemberHateoasBuilder : IHateoasSourceBuilder<Member>
    {
        public void Build(IHateoasSource<Member> source)
        {
            source.AddLink("get-member").HasRouteData(e => new { id = e.Id });
            source.AddLink("update-member").HasRouteData(e => new { id = e.Id });

            source
                .AddLink("get-guild")
                .HasRouteData(e => new { id = e.GuildId })
                .When(e => e.GuildId != null);

            source
                .AddLink("promote-member")
                .HasRouteData(e => new { id = e.Id })
                .When(e => e.GuildId != null && !e.IsGuildMaster);

            source
                .AddLink("demote-member")
                .HasRouteData(e => new { id = e.Id })
                .When(e => e.GuildId != null && e.IsGuildMaster);

            source.AddLink("leave-guild")
                    .HasRouteData(e => new { id = e.Id })
                    .When(e => e.GuildId != null);
        }
    }
}
