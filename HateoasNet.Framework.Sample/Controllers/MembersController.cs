using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HateoasNet.Framework.Sample.JsonData;
using HateoasNet.Framework.Sample.Models;

namespace HateoasNet.Framework.Sample.Controllers
{
	[RoutePrefix("members")]
	public class MembersController : ApiController
	{
		private readonly List<Member> _members;

		public MembersController(Seeder seeder)
		{
			_members = seeder.Seed<Member>();
		}

		[HttpGet]
		[Route("{id:Guid}", Name = "get-member")]
		public IHttpActionResult Get(Guid id)
		{
			var member = _members.SingleOrDefault(i => i.Id == id);
			return member != null ? Ok(member) : NotFound() as IHttpActionResult;
		}

		[HttpGet]
		[Route(Name = "get-members")]
		public IHttpActionResult GetAll(Guid? guildId = null)
		{
			var members = _members.Where(m => m.GuildId == guildId || guildId == null).ToList();
			return Ok(members);
		}

		[HttpPost]
		[Route(Name = "create-member")]
		public IHttpActionResult Create([FromBody] Member member)
		{
			return CreatedAtRoute("get-member", new {id = member.Id}, member);
		}

		[HttpPut]
		[Route("{id:Guid}", Name = "update-member")]
		public IHttpActionResult Update([FromBody] Member member)
		{
			return Ok(member);
		}

		[HttpPatch]
		[Route("{id}/promote", Name = "promote-member")]
		public IHttpActionResult Promote(Guid id)
		{
			var member = _members.SingleOrDefault(i => i.Id == id);
			if (member == null) return NotFound();
			member.IsGuildMaster = true;
			return Ok(member);
		}

		[HttpPatch]
		[Route("{id}/demote", Name = "demote-member")]
		public IHttpActionResult Demote(Guid id)
		{
			var member = _members.SingleOrDefault(i => i.Id == id);
			if (member == null) return NotFound();
			member.IsGuildMaster = false;
			return Ok(member);
		}

		[HttpPatch]
		[Route("{id}/leave", Name = "leave-guild")]
		public IHttpActionResult LeaveGuild(Guid id)
		{
			var member = _members.SingleOrDefault(i => i.Id == id);
			if (member == null) return NotFound();
			member.Guild = null;
			member.GuildId = null;
			return Ok(member);
		}
	}
}
