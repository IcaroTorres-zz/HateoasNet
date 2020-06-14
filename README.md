# HateoasNet

A small hateoas libarry for .NET with fluent-like configuration and easey integrations
with lambda expressions for building hateoas links with type mapping. Supports ease for 
custom hateoas output implementations.

### Get Started
HateoasNet can be installed using the Nuget package manager or the `dotnet` CLI.

```
dotnet add package HateoasNet
```

---

### Examples

#### Inline configuration and integration

```csharp
using HateoasNet.DependencyInjection.Core;

public void ConfigureServices(IServiceCollection services)
{
    // other registration...

    services.ConfigureHateoas(context =>
    {
        return context.Configure<List<Member>>(members =>
        {
            members.AddLink("get-members");
            members.AddLink("invite-member");
            members.AddLink("create-member");
        })

        .Configure<Member>(member =>
        {
            member.AddLink("get-member").HasRouteData(e => new { id = e.Id });
            member.AddLink("update-member").HasRouteData(e => new { id = e.Id });
        })

        .Configure<Member>(member =>
        {
            member.AddLink("get-guild")
                .HasRouteData(e => new { id = e.GuildId })
                .When(e => e.GuildId != null);

            member.AddLink("promote-member")
                .HasRouteData(e => new { id = e.Id })
                .When(e => e.GuildId != null && !e.IsGuildMaster);

            member.AddLink("demote-member")
                .HasRouteData(e => new { id = e.Id })
                .When(e => e.GuildId != null && e.IsGuildMaster);

            member.AddLink("leave-guild")
                .HasRouteData(e => new { id = e.Id })
                .When(e => e.GuildId != null);
        });
    });

    // other registrations...
}
```

#### Configuration in separated class
```csharp
using HateoasNet.Abstractions;

public class MemberHateoasBuilder : IHateoasSourceBuilder<Member>
{
    public void Build(IHateoasSource<Member> resource)
    {
        resource.AddLink("get-member").HasRouteData(e => new { id = e.Id });
        resource.AddLink("update-member").HasRouteData(e => new { id = e.Id });

        resource
            .AddLink("get-guild")
            .HasRouteData(e => new { id = e.GuildId })
            .When(e => e.GuildId != null);

        resource
            .AddLink("promote-member")
            .HasRouteData(e => new { id = e.Id })
            .When(e => e.GuildId != null && !e.IsGuildMaster);

        resource
            .AddLink("demote-member")
            .HasRouteData(e => new { id = e.Id })
            .When(e => e.GuildId != null && e.IsGuildMaster);

        resource.AddLink("leave-guild")
                .HasRouteData(e => new { id = e.Id })
                .When(e => e.GuildId != null);
    }
}
```

#### Integration calling IHateoasSourceBuilders
```csharp
using HateoasNet.DependencyInjection.Core;

public void ConfigureServices(IServiceCollection services)
{
    // other registrations...

    // setup applying map configurations from classes in separated files
    services.ConfigureHateoas(context => context
        .ApplyConfiguration(new GuildHateoasBuilder())
        .ApplyConfiguration(new ListGuildHateoasBuilder())
        .ApplyConfiguration(new MemberHateoasBuilder())
        .ApplyConfiguration(new ListMemberHateoasBuilder())
        .ApplyConfiguration(new InviteHateoasBuilder())
        .ApplyConfiguration(new InvitesHateoasBuilder()));

    // other registrations...
}
```

#### Integration using assemblies
```csharp
using HateoasNet.DependencyInjection.Core;

public void ConfigureServices(IServiceCollection services)
{
    // other registrations...

    // setup applying configurations from IHateoasSourceBuilder implementations in separated files found in a given assembly
    services.ConfigureHateoas(context => context.ConfigureFromAssembly(typeof(GuildHateoasBuilder).Assembly));

    // other registrations...
}
```

#### IHateoas Abstraction usage
```csharp
[ApiController]
[Route("[controller]")]
public class MembersController : ControllerBase
{
    private readonly IHateoas _hateoas;
    private readonly List<Member> _members;

    public MembersController(List<Member> members, IHateoas hateoas)
    {
        _hateoas = hateoas;
        _members = members;
    }

    [HttpGet("{id:guid}", Name = "get-member")]
    public IActionResult Get(Guid id)
    {
        var member = _members.SingleOrDefault(i => i.Id == id);
        var links = _hateoas.Generate(member);
        return member != null ? Ok(new { data = member, links }) : NotFound() as IActionResult;
    }
}
```

#### Sample of optional custom Hateoas output implementation
```csharp
using HateoasNet;

public class DictionaryHateoas : AbstractHateoas<ImmutableDictionary<string, object>>
{
    public DictionaryHateoas(IHateoas hateoas) : base(hateoas)
    {
    }

    public override ImmutableDictionary<string, object> GenerateCustom(IEnumerable<HateoasLink> links)
    {
        return links.ToImmutableDictionary(x => x.Rel, x => (object)new { x.Href, x.Method });
    }
}
```

#### Sample of optional custom Hateoas output registrations
```csharp
using HateoasNet.DependencyInjection.Core;

public void ConfigureServices(IServiceCollection services)
{
    // other registrations...

    // optional step to register all custom IHateoas<T> implementations in separated files found in a given assemblies
    services.RegisterAllCustomHateoas(new Assembly[] { typeof(Startup).Assembly }, ServiceLifetime.Scoped)

    // other registrations...
}
```

#### Custom IHateoas<T> Abstraction usage
```csharp
using HateoasNet.Abstractons;

[ApiController]
[Route("[controller]")]
public class GuildsController : ControllerBase
{
    private readonly IHateoas<ImmutableDictionary<string, object>> _hateoas;
    private readonly List<Guild> _guilds;

    public GuildsController(List<Guild> guilds, IHateoas<ImmutableDictionary<string, object>> hateoas)
    {
        _hateoas = hateoas;
        _guilds = guilds;
    }

    [HttpGet("{id:guid}", Name = "get-guild")]
    public IActionResult Get(Guid id)
    {
        var guild = _guilds.SingleOrDefault(i => i.Id == id);
        var links = _hateoas.Generate(guild);
        return guild != null ? Ok(new { data = guild, links }) : NotFound() as IActionResult;
    }
}
```

---

### Documentation

[Documentation still under development.](https://github.com/IcaroTorres/HateoasNet/blob/master/README.md)

### Target Platforms

HateoasNet targets NetCoreApp3.1 and Net Framework 4.72.

### Contributing

Feel free to *Fork* this repo and send a *Pull Request* with your ideas and improvements, turning this proof of concept any better.

### Credits

Project conceived by [@icarotorres](https://github.com/icarotorres "author's profile") : icaro.stuart@gmail.com, then owner of this repository.
