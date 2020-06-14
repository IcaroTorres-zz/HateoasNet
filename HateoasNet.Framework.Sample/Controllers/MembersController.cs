using HateoasNet.Abstractions;
using HateoasNet.Framework.Sample.JsonData;
using HateoasNet.Framework.Sample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace HateoasNet.Framework.Sample.Controllers
{
	[RoutePrefix("members")]
	public class MembersController : ApiController
	{
		private readonly List<Member> _members;
		private readonly IHateoas hateoas;

		public MembersController(Seeder seeder, IHateoas hateoas)
		{
			_members = seeder.Seed<Member>();
			this.hateoas = hateoas;
		}

		[HttpGet, Route("{id:guid}", Name = "get-member")]
		public IHttpActionResult Get(Guid id)
		{
			var member = _members.SingleOrDefault(i => i.Id == id);
			var links = hateoas.Generate(member);
			return member != null ? Ok(new { data = member, links }) : NotFound() as IHttpActionResult;
		}

		[HttpGet, Route(Name = "get-members")]
		public IHttpActionResult GetAll(Guid guildId)
		{
			var members = _members.Where(m => m.GuildId == guildId || guildId == Guid.Empty).ToList();
			var links = hateoas.Generate(members);
			return Ok(new { data = members, links });
		}

		[HttpPost, Route(Name = "create-member")]
		public IHttpActionResult Create([FromBody] Member member)
		{
			var links = hateoas.Generate(member);
			return CreatedAtRoute("get-member", new { id = member.Id }, new { data = member, links });
		}

		[HttpPut, Route("{id:guid}", Name = "update-member")]
		public IHttpActionResult Update([FromBody] Member member)
		{
			var links = hateoas.Generate(member);
			return Ok(new { data = member, links });
		}

		[HttpPatch, Route("{id}/promote", Name = "promote-member")]
		public IHttpActionResult Promote(Guid id)
		{
			var member = _members.SingleOrDefault(i => i.Id == id);
			if (member == null) return NotFound();
			member.IsGuildMaster = true;
			var links = hateoas.Generate(member);
			return Ok(new { data = member, links });
		}

		[HttpPatch, Route("{id}/demote", Name = "demote-member")]
		public IHttpActionResult Demote(Guid id)
		{
			var member = _members.SingleOrDefault(i => i.Id == id);
			if (member == null) return NotFound();
			member.IsGuildMaster = false;
			var links = hateoas.Generate(member);
			return Ok(new { data = member, links });
		}

		[HttpPatch, Route("{id}/leave", Name = "leave-guild")]
		public IHttpActionResult LeaveGuild(Guid id)
		{
			var member = _members.SingleOrDefault(i => i.Id == id);
			if (member == null) return NotFound();
			member.Guild = null;
			member.GuildId = null;
			var links = hateoas.Generate(member);
			return Ok(new { data = member, links });
		}
	}
}
