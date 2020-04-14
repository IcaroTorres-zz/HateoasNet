using System.Collections.Generic;
using HateoasNet.Abstractions;
using Sample.Models;

namespace Sample.HateoasMaps
{
	public class ListMemberHateoas : IHateoasBuilder<List<Member>>
	{
		public void Build(IHateoasMap<List<Member>> map)
		{
			map.HasLink("get-members");
			map.HasLink("invite-member");
			map.HasLink("create-member");
		}
	}
}