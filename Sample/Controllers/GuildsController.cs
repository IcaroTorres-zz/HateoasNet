using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Sample.JsonData;
using Sample.Models;

namespace Sample.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class GuildsController : ControllerBase
	{
		private readonly List<Guild> _guilds;

		public GuildsController(Seeder seeder)
		{
			_guilds = seeder.Seed<Guild>();
		}

		[HttpGet("{id:Guid}", Name = "get-guild")]
		public IActionResult Get(Guid id)
		{
			var guild = _guilds.SingleOrDefault(i => i.Id == id);
			return guild != null ? Ok(guild) : NotFound() as IActionResult;
		}

		[HttpGet(Name = "get-guilds")]
		public IActionResult Get()
		{
			return Ok(_guilds);
		}

		[HttpPost(Name = "create-guild")]
		public IActionResult Post([FromBody] Guild guild)
		{
			return CreatedAtRoute("get-guild", new {id = guild.Id}, guild);
		}

		[HttpPut("{id:Guid}", Name = "update-guild")]
		public IActionResult Put([FromBody] Guild guild)
		{
			return Ok(guild);
		}
	}
}