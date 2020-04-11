using System.Collections.Generic;
using HateoasNet.Core;
using HateoasNet.Core.Abstractions;
using Sample.Models;

namespace Sample.HateoasMaps
{
	public class ListGuildHateoas : IHateoasBuilder<List<Guild>>
	{
		public void Build(HateoasMap<List<Guild>> map)
		{
			map.HasLink("get-guilds");
			map.HasLink("create-guild");
		}
	}
}