using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HateoasNet.Framework.Sample.JsonData;
using HateoasNet.Framework.Sample.Models;

namespace HateoasNet.Framework.Sample.Controllers
{
	[RoutePrefix("guilds")]
	public class GuildsController : ApiController
	{
		private readonly List<Guild> _guilds;

		public GuildsController(Seeder seeder)
		{
			_guilds = seeder.Seed<Guild>();
		}

		[HttpGet]
		[Route("{id:Guid}", Name = "get-guild")]
		public IHttpActionResult Get(Guid id)
		{
			var guild = _guilds.SingleOrDefault(i => i.Id == id);
			return guild != null ? Ok(guild) : NotFound() as IHttpActionResult;
		}

		[HttpGet]
		[Route(Name = "get-guilds")]
		public IHttpActionResult Get()
		{
			return Ok(_guilds);
		}

		[HttpPost]
		[Route(Name = "create-guild")]
		public IHttpActionResult Post([FromBody] Guild guild)
		{
			return CreatedAtRoute("get-guild", new {id = guild.Id}, guild);
		}

		[HttpPut]
		[Route("{id:Guid}", Name = "update-guild")]
		public IHttpActionResult Put([FromBody] Guild guild)
		{
			return Ok(guild);
		}
	}
}