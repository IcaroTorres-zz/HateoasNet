using HateoasNet.Abstractions;
using HateoasNet.Framework.Sample.Models;

namespace HateoasNet.Framework.Sample.HateoasBuilders
{
	public class GuildHateoasBuilder : IHateoasSourceBuilder<Guild>
	{
		public void Build(IHateoasSource<Guild> resource)
		{
			resource.AddLink("get-guild").HasRouteData(g => new { id = g.Id });
			resource.AddLink("get-members").HasRouteData(g => new { guildId = g.Id });
			resource.AddLink("update-guild").HasRouteData(e => new { id = e.Id });
		}
	}
}
