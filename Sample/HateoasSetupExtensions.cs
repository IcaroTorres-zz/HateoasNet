using System.Collections.Generic;
using HateoasNet.DependencyInjection;
using HateoasNet.Resources;
using Microsoft.Extensions.DependencyInjection;
using Sample.HateoasMaps;
using Sample.Models;

namespace Sample
{
	public static class HateoasSetupExtensions
	{
		public static IMvcBuilder HateoasSampleFromAssembly(this IMvcBuilder builder)
		{
			return builder.ConfigureHateoasMap(config => 
				config.ApplyConfigurationsFromAssembly(typeof(GuildHateoas).Assembly));
		}
		
		public static IMvcBuilder HateoasSampleWithSeparatedFiles(this IMvcBuilder builder)
		{
			return builder.ConfigureHateoasMap(config =>
			{
				config.ApplyConfiguration(new GuildHateoas());
				config.ApplyConfiguration(new ListGuildHateoas());
				config.ApplyConfiguration(new MemberHateoas());
				config.ApplyConfiguration(new ListMemberHateoas());
				config.ApplyConfiguration(new InviteHateoas());
				config.ApplyConfiguration(new PaginationInviteHateoas());
			});
		}
		
		public static IMvcBuilder HateoasSampleWithManualMapping(this IMvcBuilder builder)
		{
			return builder.ConfigureHateoasMap(config =>
			{
				// map All Api returns of type List<Guild> to links with no routeData and no conditional predicate
				config.Map<List<Guild>>(guilds =>
				{
					guilds.HasLink("get-guilds");
					guilds.HasLink("create-guild");
				});

				// map All Api returns of type List<Member> to links with no routeData and no conditional predicate
				config.Map<List<Member>>(members =>
				{
					members.HasLink("get-members");
					members.HasLink("invite-member");
					members.HasLink("create-member");
				});

				// map type with links for Pagination with no routeData and no conditional predicate
				config.Map<Pagination<Invite>>(invites =>
				{
					invites.HasLink("get-invites");
					invites.HasLink("invite-member");
				});

				// map type with links for single objects using shorthand method
				config.Map<Guild>(guild =>
				{
					guild.HasLink("get-guild", g => new {id = g.Id});
					guild.HasLink("get-members", g => new {guildId = g.Id});
					guild.HasLink("update-guild", e => new {id = e.Id});
				});

				config.Map<Invite>(invite =>
				{
					// map type with links for single objects using full shorthand
					invite.HasLink("accept-invite", e => new {id = e.Id}, e => e.Status == InviteStatuses.Pending);
					invite.HasLink("decline-invite", e => new {id = e.Id}, e => e.Status == InviteStatuses.Pending);
					invite.HasLink("cancel-invite", e => new {id = e.Id}, e => e.Status == InviteStatuses.Pending);

					// map type with links for single objects using inline routeData function
					invite.HasLink("get-invite").WithData(i => new {id = i.Id});
					invite.HasLink("get-guild").WithData(i => new {id = i.GuildId});
					invite.HasLink("get-member").WithData(i => new {id = i.MemberId});
				});

				// map type with links for single objects using shorthand method
				config.Map<Member>(member =>
				{
					member.HasLink("get-member", e => new {id = e.Id});
					member.HasLink("update-member", e => new {id = e.Id});
				});
				
				// maps type with links for single objects using inline functions for routeData and conditional predicate
				config.Map<Member>(member =>
				{
					member
						.HasLink("get-guild")
						.WithData(e => new {id = e.GuildId})
						.When(e => e.GuildId != null);

					member
						.HasLink("promote-member")
						.WithData(e => new {id = e.Id})
						.When(e => e.GuildId != null && !e.IsGuildMaster);

					member
						.HasLink("demote-member")
						.WithData(e => new {id = e.Id})
						.When(e => e.GuildId != null && e.IsGuildMaster);

					member.HasLink("leave-guild")
						.WithData(e => new {id = e.Id})
						.When(e => e.GuildId != null);
				});
			});
		}

		public static IMvcBuilder HateoasSampleSimpleLinks(this IMvcBuilder builder)
		{
			return builder.ConfigureHateoasMap(config =>
			{
				// links for Lists with no routeData and no conditional predicate
				config.HasLink<List<Guild>>("get-guilds");
				config.HasLink<List<Guild>>("create-guild");
				config.HasLink<List<Member>>("get-members");
				config.HasLink<List<Member>>("invite-member");
				config.HasLink<List<Member>>("create-member");

				// links for Paginating with no routeData and no conditional predicate
				config.HasLink<Pagination<Invite>>("get-invites");
				config.HasLink<Pagination<Invite>>("invite-member");

				// links for single objects using shorthand method
				config.HasLink<Guild>("get-guild", e => new {id = e.Id});
				config.HasLink<Guild>("get-members", e => new {guildId = e.Id});
				config.HasLink<Guild>("update-guild", e => new {id = e.Id});
				config.HasLink<Member>("get-member", e => new {id = e.Id});
				config.HasLink<Member>("update-member", e => new {id = e.Id});

				// links for single objects using inline routeData function
				config
					.HasLink<Invite>("get-guild")
					.WithData(e => new {id = e.GuildId});

				config
					.HasLink<Invite>("get-member")
					.WithData(e => new {id = e.MemberId});

				config
					.HasLink<Invite>("get-invite")
					.WithData(e => new {id = e.Id});

				// links for single objects using full shorthand
				config.HasLink<Invite>("accept-invite", e => new {id = e.Id}, e => e.Status == InviteStatuses.Pending);
				config.HasLink<Invite>("decline-invite", e => new {id = e.Id}, e => e.Status == InviteStatuses.Pending);
				config.HasLink<Invite>("cancel-invite", e => new {id = e.Id}, e => e.Status == InviteStatuses.Pending);

				// links for single objects  using inline functions for routeData and conditional predicate
				config
					.HasLink<Member>("get-guild")
					.WithData(e => new {id = e.GuildId})
					.When(e => e.GuildId != null);

				config
					.HasLink<Member>("promote-member")
					.WithData(e => new {id = e.Id})
					.When(e => e.GuildId != null && !e.IsGuildMaster);

				config.HasLink<Member>("demote-member")
					.WithData(e => new {id = e.Id})
					.When(e => e.GuildId != null && e.IsGuildMaster);

				config.HasLink<Member>("leave-guild")
					.WithData(e => new {id = e.Id})
					.When(e => e.GuildId != null);
			});
		}
	}
}