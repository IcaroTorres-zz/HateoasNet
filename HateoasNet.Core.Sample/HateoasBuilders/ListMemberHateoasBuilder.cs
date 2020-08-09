using HateoasNet.Abstractions;
using HateoasNet.Core.Sample.Models;
using System.Collections.Generic;

namespace HateoasNet.Core.Sample.HateoasBuilders
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
