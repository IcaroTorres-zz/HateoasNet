using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using HateoasNet.Abstractions;
using HateoasNet.Core.Sample.JsonData;
using HateoasNet.Core.Sample.Models;
using Microsoft.AspNetCore.Mvc;

namespace HateoasNet.Core.Sample.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class GuildsController : ControllerBase
	{
		private readonly List<Guild> _guilds;
		private readonly IHateoas<ImmutableDictionary<string, object>> _hateoas;

		public GuildsController(Seeder seeder, IHateoas<ImmutableDictionary<string, object>> hateoas)
		{
			_guilds = seeder.Seed<Guild>();
			_hateoas = hateoas;
		}

		[HttpGet("{id:guid}", Name = "get-guild")]
		public IActionResult Get(Guid id)
		{
			var guild = _guilds.SingleOrDefault(i => i.Id == id);
			var links = _hateoas.Generate(guild);
			return guild != null ? Ok(new { data = guild, links }) : NotFound() as IActionResult;
		}

		[HttpGet(Name = "get-guilds")]
		public IActionResult Get()
		{
			var links = _hateoas.Generate(_guilds);
			return Ok(new { data = _guilds, links });
		}

		[HttpPost(Name = "create-guild")]
		public IActionResult Post([FromBody] Guild guild)
		{
			var links = _hateoas.Generate(guild);
			return CreatedAtRoute("get-guild", new {id = guild.Id}, new { data = guild, links });
		}

		[HttpPut("{id:guid}", Name = "update-guild")]
		public IActionResult Put([FromBody] Guild guild)
		{
			var links = _hateoas.Generate(guild);
			return Ok(new { data = guild, links });
		}
	}
}
