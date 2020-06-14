using HateoasNet.Core.Sample.HateoasBuilders;
using HateoasNet.Core.Sample.Models;
using HateoasNet.DependencyInjection.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace HateoasNet.Core.Sample
{
    public static class HateoasSetupExtensions
    {
        public static IServiceCollection HateoasConfigurationUsingAssembly(this IServiceCollection services)
        {
            // setup applying map configurations from classes in separated files found in a given assembly
            return services.ConfigureHateoas(context => context.ConfigureFromAssembly(typeof(GuildHateoasBuilder).Assembly));
        }

        public static IServiceCollection HateoasConfigurations(this IServiceCollection services)
        {
            // setup applying map configurations from classes in separated files
            return services.ConfigureHateoas(context => context
                .ApplyConfiguration(new GuildHateoasBuilder())
                .ApplyConfiguration(new ListGuildHateoasBuilder())
                .ApplyConfiguration(new MemberHateoasBuilder())
                .ApplyConfiguration(new ListMemberHateoasBuilder())
                .ApplyConfiguration(new InviteHateoasBuilder())
                .ApplyConfiguration(new InvitesHateoasBuilder()));
        }

        public static IServiceCollection HateoasInlineConfiguration(this IServiceCollection services)
        {
            return services.ConfigureHateoas(context =>
            {
                return context
                       // map All Api returns of type List<Guild> to links with no routeData and no conditional predicate
                       .Configure<List<Guild>>(guilds =>
                       {
                           guilds.AddLink("get-guilds");
                           guilds.AddLink("create-guild");
                       })

                       // map All Api returns of type List<Member> to links with no routeData and no conditional predicate
                       .Configure<List<Member>>(members =>
                       {
                           members.AddLink("get-members");
                           members.AddLink("invite-member");
                           members.AddLink("create-member");
                       })

                       // map type with links for Pagination with no routeData and no conditional predicate
                       .Configure<List<Invite>>(invites =>
                       {
                           invites.AddLink("get-invites");
                           invites.AddLink("invite-member");
                       })

                       // map type with links for single objects
                       .Configure<Guild>(guild =>
                       {
                           guild.AddLink("get-guild").HasRouteData(g => new { id = g.Id });
                           guild.AddLink("get-members").HasRouteData(g => new { guildId = g.Id });
                           guild.AddLink("update-guild").HasRouteData(e => new { id = e.Id });
                       })
                       .Configure<Invite>(invite =>
                       {
                           invite.AddLink("accept-invite")
                                 .HasRouteData(e => new { id = e.Id })
                                 .When(e => e.Status == InviteStatuses.Pending);

                           invite.AddLink("decline-invite")
                                 .HasRouteData(e => new { id = e.Id })
                                 .When(e => e.Status == InviteStatuses.Pending);

                           invite.AddLink("cancel-invite")
                                 .HasRouteData(e => new { id = e.Id })
                                 .When(e => e.Status == InviteStatuses.Pending);

                           invite.AddLink("get-invite").HasRouteData(i => new { id = i.Id });
                           invite.AddLink("get-guild").HasRouteData(i => new { id = i.GuildId });
                           invite.AddLink("get-member").HasRouteData(i => new { id = i.MemberId });
                       })
                       .Configure<Member>(member =>
                       {
                           member.AddLink("get-member").HasRouteData(e => new { id = e.Id });
                           member.AddLink("update-member").HasRouteData(e => new { id = e.Id });
                       })
                       .Configure<Member>(member =>
                       {
                           member
                               .AddLink("get-guild")
                               .HasRouteData(e => new { id = e.GuildId })
                               .When(e => e.GuildId != null);

                           member
                               .AddLink("promote-member")
                               .HasRouteData(e => new { id = e.Id })
                               .When(e => e.GuildId != null && !e.IsGuildMaster);

                           member
                               .AddLink("demote-member")
                               .HasRouteData(e => new { id = e.Id })
                               .When(e => e.GuildId != null && e.IsGuildMaster);

                           member.AddLink("leave-guild")
                                 .HasRouteData(e => new { id = e.Id })
                                 .When(e => e.GuildId != null);
                       });
            });
        }
    }
}
