using HateoasNet.Abstractions;
using HateoasNet.Core.Sample.Models;
using HateoasNet.Resources;

namespace HateoasNet.Core.Sample.HateoasConfigurations
{
    public class PaginationInviteHateoasConfiguration : IHateoasResourceConfiguration<Pagination<Invite>>
    {
        public void Configure(IHateoasResource<Pagination<Invite>> resource)
        {
            resource.HasLink("get-invites");
            resource.HasLink("invite-member");
        }
    }
}