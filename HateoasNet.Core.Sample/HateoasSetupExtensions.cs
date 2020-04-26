﻿using System.Collections.Generic;
using HateoasNet.Core.DependencyInjection;
using HateoasNet.Core.Sample.HateoasConfigurations;
using HateoasNet.Core.Sample.Models;
using HateoasNet.Resources;
using Microsoft.Extensions.DependencyInjection;

namespace HateoasNet.Core.Sample
{
	public static class HateoasSetupExtensions
	{
		public static IServiceCollection HateoasConfigurationUsingAssembly(this IServiceCollection services)
		{
			// setup applying map configurations from classes in separated files found in a given assembly
			return services
				.ConfigureHateoas(context => context.ConfigureFromAssembly(typeof(GuildHateoasConfiguration).Assembly));
		}

		public static IServiceCollection HateoasConfigurations(this IServiceCollection services)
		{
			// setup applying map configurations from classes in separated files
			return services.ConfigureHateoas(context => context
			                                           .ApplyConfiguration(new GuildHateoasConfiguration())
			                                           .ApplyConfiguration(new ListGuildHateoasConfiguration())
			                                           .ApplyConfiguration(new MemberHateoasConfiguration())
			                                           .ApplyConfiguration(new ListMemberHateoasConfiguration())
			                                           .ApplyConfiguration(new InviteHateoasConfiguration())
			                                           .ApplyConfiguration(new PaginationInviteHateoasConfiguration()));
		}

		public static IServiceCollection HateoasInlineConfiguration(this IServiceCollection services)
		{
			return services.ConfigureHateoas(context =>
			{
				return context
				       // map All Api returns of type List<Guild> to links with no routeData and no conditional predicate
				       .Configure<List<Guild>>(guilds =>
				       {
					       guilds.HasLink("get-guilds");
					       guilds.HasLink("create-guild");
				       })

				       // map All Api returns of type List<Member> to links with no routeData and no conditional predicate
				       .Configure<List<Member>>(members =>
				       {
					       members.HasLink("get-members");
					       members.HasLink("invite-member");
					       members.HasLink("create-member");
				       })

				       // map type with links for Pagination with no routeData and no conditional predicate
				       .Configure<Pagination<Invite>>(invites =>
				       {
					       invites.HasLink("get-invites");
					       invites.HasLink("invite-member");
				       })

				       // map type with links for single objects
				       .Configure<Guild>(guild =>
				       {
					       guild.HasLink("get-guild").HasRouteData(g => new {id = g.Id});
					       guild.HasLink("get-members").HasRouteData(g => new {guildId = g.Id});
					       guild.HasLink("update-guild").HasRouteData(e => new {id = e.Id});
				       })
				       .Configure<Invite>(invite =>
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
				       .Configure<Member>(member =>
				       {
					       member.HasLink("get-member").HasRouteData(e => new {id = e.Id});
					       member.HasLink("update-member").HasRouteData(e => new {id = e.Id});
				       })
				       .Configure<Member>(member =>
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
