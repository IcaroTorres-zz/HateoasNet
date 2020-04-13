using System.Collections.Generic;
using HateoasNet.Abstractions;
using HateoasNet.Core;
using Sample.Models;

namespace Sample.HateoasMaps
{
	public class ListMemberHateoas : IHateoasBuilder<List<Member>>
	{
		public void Build(HateoasMap<List<Member>> map)
		{
			map.HasLink("get-members");
			map.HasLink("invite-member");
			map.HasLink("create-member");
		}
	}
}