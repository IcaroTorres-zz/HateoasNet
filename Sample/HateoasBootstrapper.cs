using System.Collections.Generic;
using HateoasNet.DependencyInjection;
using HateoasNet.Resources;
using Microsoft.Extensions.DependencyInjection;
using Sample.Models;

namespace Sample
{
	public static class HateoasExtensions
	{
		public static IMvcBuilder BootstrapHateoasFormatter(this IMvcBuilder builder)
		{
			return builder.EnableHateoas(options =>
			{
				// links for Lists with no routeData and no conditional predicate
				options.EnableLinkFor<List<Guild>>("get-guilds");
				options.EnableLinkFor<List<Guild>>("create-guild");
				options.EnableLinkFor<List<Member>>("get-members");
				options.EnableLinkFor<List<Member>>("invite-member");
				options.EnableLinkFor<List<Member>>("create-member");

				// links for Paginating with no routeData and no conditional predicate
				options.EnableLinkFor<Pagination<Invite>>("get-invites");
				options.EnableLinkFor<Pagination<Invite>>("invite-member");

				// links for single objects using shorthand method
				options.EnableLinkFor<Guild>("get-guild", e => new {id = e.Id});
				options.EnableLinkFor<Guild>("get-members", e => new {guildId = e.Id});
				options.EnableLinkFor<Guild>("update-guild", e => new {id = e.Id});
				options.EnableLinkFor<Member>("get-member", e => new {id = e.Id});
				options.EnableLinkFor<Member>("update-member", e => new {id = e.Id});

				// links for single objects using inline routeData function
				options
					.EnableLinkFor<Invite>("get-guild")
					.WithRouteData(e => new {id = e.GuildId});
				
				options
					.EnableLinkFor<Invite>("get-member")
					.WithRouteData(e => new {id = e.MemberId});
				
				options
					.EnableLinkFor<Invite>("get-invite")
					.WithRouteData(e => new {id = e.Id});

				// links for single objects using full shorthand
				options.EnableLinkFor<Invite>("accept-invite", e => new {id = e.Id}, e => e.Status == InviteStatuses.Pending);
				options.EnableLinkFor<Invite>("decline-invite", e => new {id = e.Id}, e => e.Status == InviteStatuses.Pending);
				options.EnableLinkFor<Invite>("cancel-invite", e => new {id = e.Id}, e => e.Status == InviteStatuses.Pending);

				// links for single objects  using inline functions for routeData and conditional predicate
				options
					.EnableLinkFor<Member>("get-guild")
					.WithRouteData(e => new {id = e.GuildId})
					.DisplayedWhen(e => e.GuildId != null);

				options
					.EnableLinkFor<Member>("promote-member")
					.WithRouteData(e => new {id = e.Id})
					.DisplayedWhen(e => e.GuildId != null && !e.IsGuildMaster);

				options.EnableLinkFor<Member>("demote-member")
					.WithRouteData(e => new {id = e.Id})
					.DisplayedWhen(e => e.GuildId != null && e.IsGuildMaster);

				options.EnableLinkFor<Member>("leave-guild")
					.WithRouteData(e => new {id = e.Id})
					.DisplayedWhen(e => e.GuildId != null);
			});
		}
	}
}