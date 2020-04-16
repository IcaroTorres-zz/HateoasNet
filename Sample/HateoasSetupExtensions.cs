using System.Collections.Generic;
using HateoasNet.Core.DependencyInjection;
using HateoasNet.Resources;
using Microsoft.Extensions.DependencyInjection;
using Sample.HateoasMaps;
using Sample.Models;

namespace Sample
{
	public static class HateoasSetupExtensions
	{
		public static IServiceCollection HateoasSeparatedFilesUsingAssembly(this IServiceCollection services)
		{
			// setup applying map configurations from classes in separated files found in a given assembly
			return services.ConfigureHateoasMap(
				config => config.ApplyConfigurationsFromAssembly(typeof(GuildHateoas).Assembly));
		}

		public static IServiceCollection HateoasSeparatedFilesMapping(this IServiceCollection services)
		{
			// setup applying map configurations from classes in separated files
			return services.ConfigureHateoasMap(config =>
			{
				config.ApplyConfiguration(new GuildHateoas())
					.ApplyConfiguration(new ListGuildHateoas())
					.ApplyConfiguration(new MemberHateoas())
					.ApplyConfiguration(new ListMemberHateoas())
					.ApplyConfiguration(new InviteHateoas())
					.ApplyConfiguration(new PaginationInviteHateoas());
			});
		}

		public static IServiceCollection HateoasOneFileMapping(this IServiceCollection services)
		{
			return services.ConfigureHateoasMap(config =>
			{
				config
					// map All Api returns of type List<Guild> to links with no routeData and no conditional predicate
					.Map<List<Guild>>(guilds =>
					{
						guilds.HasLink("get-guilds");
						guilds.HasLink("create-guild");
					})

					// map All Api returns of type List<Member> to links with no routeData and no conditional predicate
					.Map<List<Member>>(members =>
					{
						members.HasLink("get-members");
						members.HasLink("invite-member");
						members.HasLink("create-member");
					})

					// map type with links for Pagination with no routeData and no conditional predicate
					.Map<Pagination<Invite>>(invites =>
					{
						invites.HasLink("get-invites");
						invites.HasLink("invite-member");
					})

					// map type with links for single objects
					.Map<Guild>(guild =>
					{
						guild.HasLink("get-guild").HasRouteData(g => new {id = g.Id});
						guild.HasLink("get-members").HasRouteData(g => new {guildId = g.Id});
						guild.HasLink("update-guild").HasRouteData(e => new {id = e.Id});
					})
					
					.Map<Invite>(invite =>
					{
						invite.HasLink("accept-invite")
							.HasRouteData(e => new {id = e.Id})
							.HasConditional(e => e.Status == InviteStatuses.Pending);
						
						invite.HasLink("decline-invite")
							.HasRouteData(e => new {id = e.Id})
							.HasConditional(e => e.Status == InviteStatuses.Pending);
						
						invite.HasLink("cancel-invite")
							.HasRouteData(e => new {id = e.Id})
							.HasConditional(e => e.Status == InviteStatuses.Pending);

						invite.HasLink("get-invite").HasRouteData(i => new {id = i.Id});
						invite.HasLink("get-guild").HasRouteData(i => new {id = i.GuildId});
						invite.HasLink("get-member").HasRouteData(i => new {id = i.MemberId});
					})
					
					.Map<Member>(member =>
					{
						member.HasLink("get-member").HasRouteData(e => new {id = e.Id});
						member.HasLink("update-member").HasRouteData(e => new {id = e.Id});
					})
					
					.Map<Member>(member =>
					{
						member
							.HasLink("get-guild")
							.HasRouteData(e => new {id = e.GuildId})
							.HasConditional(e => e.GuildId != null);

						member
							.HasLink("promote-member")
							.HasRouteData(e => new {id = e.Id})
							.HasConditional(e => e.GuildId != null && !e.IsGuildMaster);

						member
							.HasLink("demote-member")
							.HasRouteData(e => new {id = e.Id})
							.HasConditional(e => e.GuildId != null && e.IsGuildMaster);

						member.HasLink("leave-guild")
							.HasRouteData(e => new {id = e.Id})
							.HasConditional(e => e.GuildId != null);
					});
			});
		}
	}
}