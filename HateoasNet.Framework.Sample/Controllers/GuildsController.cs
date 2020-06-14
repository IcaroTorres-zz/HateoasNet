using HateoasNet.Abstractions;
using HateoasNet.Framework.Sample.JsonData;
using HateoasNet.Framework.Sample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace HateoasNet.Framework.Sample.Controllers
{
	[RoutePrefix("some-prefix/guilds")]
	public class GuildsController : ApiController
	{
		private readonly List<Guild> _guilds;
		private readonly IHateoas _hateoas;

		public GuildsController(Seeder seeder, IHateoas hateoas)
		{
			_guilds = seeder.Seed<Guild>();
			_hateoas = hateoas;
		}

		[HttpGet, Route("{id:guid}", Name = "get-guild")]
		public IHttpActionResult Get(Guid id)
		{
			var guild = _guilds.SingleOrDefault(i => i.Id == id);
			var links = _hateoas.Generate(guild);
			return guild != null ? Ok(new { data = guild, links }) : NotFound() as IHttpActionResult;
		}

		[HttpGet, Route(Name = "get-guilds")]
		public IHttpActionResult Get()
		{
			var links = _hateoas.Generate(_guilds);
			return Ok(new { data = _guilds, links });
		}

		[HttpPost, Route(Name = "create-guild")]
		public IHttpActionResult Post([FromBody] Guild guild)
		{
			var links = _hateoas.Generate(guild);
			return CreatedAtRoute("get-guild", new { id = guild.Id }, new { data = guild, links });
		}

		[HttpPut, Route("{id:guid}", Name = "update-guild")]
		public IHttpActionResult Put([FromBody] Guild guild)
		{
			var links = _hateoas.Generate(guild);
			return Ok(new { data = guild, links });
		}
	}
}
