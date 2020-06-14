using System;
using System.Collections.Generic;
using System.Linq;
using HateoasNet.Abstractions;
using HateoasNet.Core.Sample.JsonData;
using HateoasNet.Core.Sample.Models;
using Microsoft.AspNetCore.Mvc;

namespace HateoasNet.Core.Sample.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class MembersController : ControllerBase
	{
		private readonly List<Member> _members;
		private readonly IHateoas hateoas;

		public MembersController(Seeder seeder, IHateoas hateoas)
		{
			_members = seeder.Seed<Member>();
			this.hateoas = hateoas;
		}

		[HttpGet("{id:Guid}", Name = "get-member")]
		public IActionResult Get(Guid id)
		{
			var member = _members.SingleOrDefault(i => i.Id == id);
			var links = hateoas.Generate(member);
			return member != null ? Ok(new { data = member, links }) : NotFound() as IActionResult;
		}

		[HttpGet(Name = "get-members")]
		public IActionResult GetAll([FromQuery] Guid guildId)
		{
			var members = _members.Where(m => m.GuildId == guildId || guildId == Guid.Empty).ToList();
			var links = hateoas.Generate(members);
			return Ok(new { data = members, links });
		}

		[HttpPost(Name = "create-member")]
		public IActionResult Create([FromBody] Member member)
		{
			var links = hateoas.Generate(member);
			return CreatedAtRoute("get-member", new {id = member.Id}, new { data = member, links });
		}

		[HttpPut("{id:Guid}", Name = "update-member")]
		public IActionResult Update([FromBody] Member member)
		{
			var links = hateoas.Generate(member);
			return Ok(new { data = member, links });
		}

		[HttpPatch("{id}/promote", Name = "promote-member")]
		public IActionResult Promote(Guid id)
		{
			var member = _members.SingleOrDefault(i => i.Id == id);
			if (member == null) return NotFound();
			member.IsGuildMaster = true;
			var links = hateoas.Generate(member);
			return Ok(new { data = member, links });
		}

		[HttpPatch("{id}/demote", Name = "demote-member")]
		public IActionResult Demote(Guid id)
		{
			var member = _members.SingleOrDefault(i => i.Id == id);
			if (member == null) return NotFound();
			member.IsGuildMaster = false;
			var links = hateoas.Generate(member);
			return Ok(new { data = member, links });
		}

		[HttpPatch("{id}/leave", Name = "leave-guild")]
		public IActionResult LeaveGuild(Guid id)
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
