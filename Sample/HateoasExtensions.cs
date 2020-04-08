using System;
using System.Collections.Generic;
using HateoasNet;
using HateoasNet.Resources;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Sample.Models;

namespace Sample
{
	public static class HateoasExtensions
	{
		private static IMvcBuilder EnableHateoasOutput(this IMvcBuilder builder, Action<HateoasNetOptions> options = null)
		{
			if (options != null) builder.Services.Configure(options);
			builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
			builder.AddMvcOptions(o => o.OutputFormatters.Add(new JsonHateoasFormatter()));
			return builder;
		}

		public static IMvcBuilder BootstrapHateoasFormatter(this IMvcBuilder builder)
		{
			return builder.EnableHateoasOutput(options => options
				.AddLink<List<Guild>>("get-guilds")
				.AddLink<List<Guild>>("create-guild")
				.AddLink<Guild>("get-guild", e => new {id = e.Id})
				.AddLink<Guild>("get-members", e => new {guildId = e.Id})
				.AddLink<Guild>("update-guild", e => new {id = e.Id})
				.AddLink<List<Member>>("get-members")
				.AddLink<List<Member>>("create-member")
				.AddLink<List<Member>>("invite-member")
				.AddLink<Member>("get-member", e => new {id = e.Id})
				.AddLink<Member>("get-guild", e => new {id = e.GuildId}, e => e.GuildId != null)
				.AddLink<Member>("update-member", e => new {id = e.Id})
				.AddLink<Member>("promote-member", e => new {id = e.Id}, e => e.GuildId != null && !e.IsGuildMaster)
				.AddLink<Member>("demote-member", e => new {id = e.Id}, e => e.GuildId != null && e.IsGuildMaster)
				.AddLink<Member>("leave-guild", e => new {id = e.Id}, e => e.GuildId != null)
				.AddLink<Pagination<Invite>>("get-invites")
				.AddLink<Pagination<Invite>>("invite-member")
				.AddLink<Invite>("get-invite", e => new {id = e.Id})
				.AddLink<Invite>("accept-invite", e => new {id = e.Id}, e => e.Status == InviteStatuses.Pending)
				.AddLink<Invite>("decline-invite", e => new {id = e.Id}, e => e.Status == InviteStatuses.Pending)
				.AddLink<Invite>("cancel-invite", e => new {id = e.Id}, e => e.Status == InviteStatuses.Pending)
				.AddLink<Invite>("get-guild", e => new {id = e.GuildId})
				.AddLink<Invite>("get-member", e => new {id = e.MemberId}));
		}
	}
}