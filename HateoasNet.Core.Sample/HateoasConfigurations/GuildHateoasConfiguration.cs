using HateoasNet.Abstractions;
using HateoasNet.Core.Sample.Models;

namespace HateoasNet.Core.Sample.HateoasConfigurations
{
	public class GuildHateoasConfiguration : IHateoasResourceConfiguration<Guild>
	{
		public void Configure(IHateoasResource<Guild> resource)
		{
			resource.HasLink("get-guild").HasRouteData(g => new {id = g.Id});
			resource.HasLink("get-members").HasRouteData(g => new {guildId = g.Id});
			resource.HasLink("update-guild").HasRouteData(e => new {id = e.Id});
		}
	}
}