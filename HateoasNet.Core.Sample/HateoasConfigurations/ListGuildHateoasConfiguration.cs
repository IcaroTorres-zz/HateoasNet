using HateoasNet.Abstractions;
using HateoasNet.Core.Sample.Models;
using System.Collections.Generic;

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