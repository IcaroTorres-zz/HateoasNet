using HateoasNet.Abstractions;
using HateoasNet.Core.Sample.Models;

namespace HateoasNet.Core.Sample.HateoasBuilders
{
    public class GuildHateoasBuilder : IHateoasSourceBuilder<Guild>
    {
        public void Build(IHateoasSource<Guild> source)
        {
            source.AddLink("get-guild").HasRouteData(g => new { id = g.Id }).PresentedAs("getGuild");
            source.AddLink("get-members").HasRouteData(g => new { guildId = g.Id });
            source.AddLink("update-guild").HasRouteData(e => new { id = e.Id });
        }
    }
}
