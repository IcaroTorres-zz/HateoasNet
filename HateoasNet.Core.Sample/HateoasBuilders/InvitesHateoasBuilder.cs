using HateoasNet.Abstractions;
using HateoasNet.Core.Sample.Models;
using System.Collections.Generic;

namespace HateoasNet.Core.Sample.HateoasBuilders
{
    public class InvitesHateoasBuilder : IHateoasSourceBuilder<List<Invite>>
    {
        public void Build(IHateoasSource<List<Invite>> resource)
        {
            resource.AddLink("get-invites");
            resource.AddLink("invite-member");
        }
    }
}
