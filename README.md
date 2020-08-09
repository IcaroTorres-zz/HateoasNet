# HateoasNet

A small hateoas libarry for .NET with fluent-like configuration and easey integrations
with lambda expressions for building hateoas links with type mapping. Supports ease for 
custom hateoas output implementations.

### Status

#### Github Actions 
![CI](https://github.com/IcaroTorres/HateoasNet/workflows/CI/badge.svg)

#### Sonar Cloud
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=hateoas-net-f500510f-d3cc-4979-8ba0-2e70d2c15da8&metric=alert_status)](https://sonarcloud.io/dashboard?id=hateoas-net-f500510f-d3cc-4979-8ba0-2e70d2c15da8)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=hateoas-net-f500510f-d3cc-4979-8ba0-2e70d2c15da8&metric=coverage)](https://sonarcloud.io/dashboard?id=hateoas-net-f500510f-d3cc-4979-8ba0-2e70d2c15da8)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=hateoas-net-f500510f-d3cc-4979-8ba0-2e70d2c15da8&metric=bugs)](https://sonarcloud.io/dashboard?id=hateoas-net-f500510f-d3cc-4979-8ba0-2e70d2c15da8)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=hateoas-net-f500510f-d3cc-4979-8ba0-2e70d2c15da8&metric=code_smells)](https://sonarcloud.io/dashboard?id=hateoas-net-f500510f-d3cc-4979-8ba0-2e70d2c15da8)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=hateoas-net-f500510f-d3cc-4979-8ba0-2e70d2c15da8&metric=vulnerabilities)](https://sonarcloud.io/dashboard?id=hateoas-net-f500510f-d3cc-4979-8ba0-2e70d2c15da8)

### Target Platforms

HateoasNet targets NetCoreApp3.1 and Net Framework 4.72.

### Get Started

The latest stable version of HateoasNet can be installed using the Nuget package manager
```
Install-Package HateoasNet -Version 2.0.1
```

`Dotnet` CLI
```
dotnet add package HateoasNet --version 2.0.1
```

or package reference.
```
<PackageReference Include="HateoasNet" Version="2.0.1" />
```

---

### Contributing

Before contributing, get started with our [Contributing guide](https://github.com/IcaroTorres/HateoasNet/blob/master/CONTRIBUTING.md).

### Code of conduct

Please note we have a [Code of conduct](https://github.com/IcaroTorres/HateoasNet/blob/master/CODE_OF_CONDUCT.md), please follow it in all your interactions with the project.

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
using HateoasNet;
using System.Linq;
using System.Collection.Generics;

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
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;

public class DictionaryHateoas : AbstractHateoas<ImmutableDictionary<string, object>>
{
    public DictionaryHateoas(IHateoas hateoas) : base(hateoas)
    {
    }

    protected override ImmutableDictionary<string, object> GenerateCustom(IEnumerable<HateoasLink> links)
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
using HateoasNet;
using System.Linq;
using System.Collections.Generic;

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

### Credits

Project conceived by [@icarotorres](https://github.com/icarotorres "author's profile") : icaro.stuart@gmail.com, then owner of this repository.