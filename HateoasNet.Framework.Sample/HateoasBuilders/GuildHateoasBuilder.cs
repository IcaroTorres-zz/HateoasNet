using HateoasNet.Abstractions;
using HateoasNet.Framework.Sample.Models;

namespace HateoasNet.Framework.Sample.HateoasBuilders
{
    public class GuildHateoasBuilder : IHateoasSourceBuilder<Guild>
    {
        public void Build(IHateoasSource<Guild> source)
        {
            source.AddLink("get-guild").HasRouteData(g => new { id = g.Id });
            source.AddLink("get-members").HasRouteData(g => new { guildId = g.Id });
            source.AddLink("update-guild").HasRouteData(e => new { id = e.Id });
        }
    }
}
