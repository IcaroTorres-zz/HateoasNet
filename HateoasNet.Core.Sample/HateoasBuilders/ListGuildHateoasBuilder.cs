using HateoasNet.Abstractions;
using HateoasNet.Core.Sample.Models;
using System.Collections.Generic;

namespace HateoasNet.Core.Sample.HateoasBuilders
{
    public class ListGuildHateoasBuilder : IHateoasSourceBuilder<List<Guild>>
    {
        public void Build(IHateoasSource<List<Guild>> source)
        {
            source.AddLink("get-guilds");
            source.AddLink("create-guild");
        }
    }
}
