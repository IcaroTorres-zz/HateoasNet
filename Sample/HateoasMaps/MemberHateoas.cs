using HateoasNet.Core;
using HateoasNet.Core.Abstractions;
using Sample.Models;

namespace Sample.HateoasMaps
{
	public class MemberHateoas : IHateoasBuilder<Member>
	{
		public void Build(HateoasMap<Member> map)
		{
			map.HasLink("get-member", e => new {id = e.Id});
			map.HasLink("update-member", e => new {id = e.Id});
			
			map
				.HasLink("get-guild")
				.WithData(e => new {id = e.GuildId})
				.When(e => e.GuildId != null);

			map
				.HasLink("promote-member")
				.WithData(e => new {id = e.Id})
				.When(e => e.GuildId != null && !e.IsGuildMaster);

			map
				.HasLink("demote-member")
				.WithData(e => new {id = e.Id})
				.When(e => e.GuildId != null && e.IsGuildMaster);

			map.HasLink("leave-guild")
				.WithData(e => new {id = e.Id})
				.When(e => e.GuildId != null);
		}
	}
}