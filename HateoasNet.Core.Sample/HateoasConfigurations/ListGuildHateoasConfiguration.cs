using System.Collections.Generic;
using HateoasNet.Abstractions;
using HateoasNet.Core.Sample.Models;

namespace HateoasNet.Core.Sample.HateoasConfigurations
{
	public class ListGuildHateoasConfiguration : IHateoasResourceConfiguration<List<Guild>>
	{
		public void Configure(IHateoasResource<List<Guild>> resource)
		{
			resource.HasLink("get-guilds");
			resource.HasLink("create-guild");
		}
	}
}
