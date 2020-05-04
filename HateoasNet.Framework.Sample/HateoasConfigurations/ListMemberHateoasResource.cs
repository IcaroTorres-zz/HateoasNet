using HateoasNet.Abstractions;
using HateoasNet.Framework.Sample.Models;
using System.Collections.Generic;

namespace HateoasNet.Framework.Sample.HateoasConfigurations
{
    public class ListMemberHateoasResource : IHateoasResourceConfiguration<List<Member>>
    {
        public void Configure(IHateoasResource<List<Member>> resource)
        {
            resource.HasLink("get-members");
            resource.HasLink("invite-member");
            resource.HasLink("create-member");
        }
    }
}