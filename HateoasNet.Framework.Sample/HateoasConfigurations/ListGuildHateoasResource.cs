using HateoasNet.Abstractions;
using HateoasNet.Framework.Sample.Models;
using System.Collections.Generic;

namespace HateoasNet.Framework.Sample.HateoasConfigurations
{
    public class ListGuildHateoasResource : IHateoasResourceConfiguration<List<Guild>>
    {
        public void Configure(IHateoasResource<List<Guild>> resource)
        {
            resource.HasLink("get-guilds");
            resource.HasLink("create-guild");
        }
    }
}