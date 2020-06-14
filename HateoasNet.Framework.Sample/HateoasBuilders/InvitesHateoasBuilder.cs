using HateoasNet.Abstractions;
using HateoasNet.Framework.Sample.Models;
using System.Collections.Generic;

namespace HateoasNet.Framework.Sample.HateoasBuilders
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
