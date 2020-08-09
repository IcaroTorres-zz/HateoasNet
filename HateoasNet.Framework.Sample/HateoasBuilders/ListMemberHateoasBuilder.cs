using HateoasNet.Abstractions;
using HateoasNet.Framework.Sample.Models;
using System.Collections.Generic;

namespace HateoasNet.Framework.Sample.HateoasBuilders
{
    public class ListMemberHateoasBuilder : IHateoasSourceBuilder<List<Member>>
    {
        public void Build(IHateoasSource<List<Member>> source)
        {
            source.AddLink("get-members");
            source.AddLink("invite-member");
            source.AddLink("create-member");
        }
    }
}
