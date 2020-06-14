using HateoasNet.Abstractions;
using HateoasNet.Core.Sample.Models;
using System.Collections.Generic;

namespace HateoasNet.Core.Sample.HateoasBuilders
{
    public class ListGuildHateoasBuilder : IHateoasSourceBuilder<List<Guild>>
    {
        public void Build(IHateoasSource<List<Guild>> resource)
        {
            resource.AddLink("get-guilds");
            resource.AddLink("create-guild");
        }
    }
}
