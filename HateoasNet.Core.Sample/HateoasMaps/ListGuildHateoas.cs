using System.Collections.Generic;
using HateoasNet.Abstractions;
using HateoasNet.Core.Sample.Models;

namespace HateoasNet.Core.Sample.HateoasMaps
{
	public class ListGuildHateoas : IHateoasBuilder<List<Guild>>
	{
		public void Build(IHateoasMap<List<Guild>> map)
		{
			map.HasLink("get-guilds");
			map.HasLink("create-guild");
		}
	}
}