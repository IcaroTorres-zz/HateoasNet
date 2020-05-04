using HateoasNet.Abstractions;
using HateoasNet.Framework.Sample.Models;

namespace HateoasNet.Framework.Sample.HateoasConfigurations
{
    public class GuildHateoasResource : IHateoasResourceConfiguration<Guild>
    {
        public void Configure(IHateoasResource<Guild> resource)
        {
            resource.HasLink("get-guild").HasRouteData(g => new { id = g.Id });
            resource.HasLink("get-members").HasRouteData(g => new { guildId = g.Id });
            resource.HasLink("update-guild").HasRouteData(e => new { id = e.Id });
        }
    }
}