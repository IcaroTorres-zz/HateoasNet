using HateoasNet.Abstractions;
using HateoasNet.Framework.Sample.Models;
using System.Collections.Generic;

namespace HateoasNet.Framework.Sample.HateoasBuilders
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
