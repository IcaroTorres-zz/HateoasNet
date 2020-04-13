﻿using HateoasNet.Core;
using HateoasNet.Core.Abstractions;
using Sample.Models;

namespace Sample.HateoasMaps
{
	public class GuildHateoas : IHateoasBuilder<Guild>
	{
		public void Build(HateoasMap<Guild> map)
		{
			map.HasLink("get-guild", g => new {id = g.Id});
			map.HasLink("get-members", g => new {guildId = g.Id});
			map.HasLink("update-guild", e => new {id = e.Id});
		}
	}
}