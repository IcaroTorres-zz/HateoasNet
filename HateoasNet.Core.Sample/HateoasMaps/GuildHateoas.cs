﻿using HateoasNet.Abstractions;
using HateoasNet.Core.Sample.Models;

namespace HateoasNet.Core.Sample.HateoasMaps
{
	public class GuildHateoas : IHateoasBuilder<Guild>
	{
		public void Build(IHateoasMap<Guild> map)
		{
			map.HasLink("get-guild").HasRouteData(g => new {id = g.Id});
			map.HasLink("get-members").HasRouteData(g => new {guildId = g.Id});
			map.HasLink("update-guild").HasRouteData(e => new {id = e.Id});
		}
	}
}