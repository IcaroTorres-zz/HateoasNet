using HateoasNet.Abstractions;
using HateoasNet.Framework.Sample.Models;
using System.Collections.Generic;

namespace HateoasNet.Framework.Sample.HateoasBuilders
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
