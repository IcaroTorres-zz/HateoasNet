using HateoasNet.Abstractions;
using HateoasNet.Framework.Sample.Models;

namespace HateoasNet.Framework.Sample.HateoasMaps
{
	public class MemberHateoas : IHateoasBuilder<Member>
	{
		public void Build(IHateoasMap<Member> map)
		{
			map.HasLink("get-member").HasRouteData(e => new {id = e.Id});
			map.HasLink("update-member").HasRouteData(e => new {id = e.Id});

			map
				.HasLink("get-guild")
				.HasRouteData(e => new {id = e.GuildId})
				.HasConditional(e => e.GuildId != null);

			map
				.HasLink("promote-member")
				.HasRouteData(e => new {id = e.Id})
				.HasConditional(e => e.GuildId != null && !e.IsGuildMaster);

			map
				.HasLink("demote-member")
				.HasRouteData(e => new {id = e.Id})
				.HasConditional(e => e.GuildId != null && e.IsGuildMaster);

			map.HasLink("leave-guild")
				.HasRouteData(e => new {id = e.Id})
				.HasConditional(e => e.GuildId != null);
		}
	}
}