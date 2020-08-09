using HateoasNet.Abstractions;
using HateoasNet.Core.Sample.Models;
using System.Collections.Generic;

namespace HateoasNet.Core.Sample.HateoasBuilders
{
    public class InvitesHateoasBuilder : IHateoasSourceBuilder<List<Invite>>
    {
        public void Build(IHateoasSource<List<Invite>> source)
        {
            source.AddLink("get-invites");
            source.AddLink("invite-member");
        }
    }
}
