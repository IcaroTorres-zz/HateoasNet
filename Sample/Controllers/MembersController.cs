﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sample.JsonData;
using Sample.Models;

namespace Sample.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class MembersController : ControllerBase
	{
		private readonly List<Member> _members = new List<Member>();

		public MembersController(ILogger<GuildsController> logger)
		{
			_members = Seeder.Seed<Member>();
		}

		[HttpGet("{id:Guid}", Name = "get-member")]
		public IActionResult Get(Guid id)
		{
			var member = _members.SingleOrDefault(i => i.Id == id);
			return member != null ? Ok(member) : NotFound() as IActionResult;
		}

		[HttpGet(Name = "get-members")]
		public IActionResult GetAll([FromQuery] Guid guildId)
		{
			var members = _members.Where(m => m.GuildId == guildId || guildId == Guid.Empty).ToList();
			return Ok(members);
		}

		[HttpPost(Name = "create-member")]
		public IActionResult Create([FromBody] Member member)
		{
			return CreatedAtRoute("get-member", new {id = member.Id}, member);
		}

		[HttpPut("{id:Guid}", Name = "update-member")]
		public IActionResult Update([FromBody] Member member)
		{
			return Ok(member);
		}

		[HttpPatch("{id}/promote", Name = "promote-member")]
		public IActionResult Promote(Guid id)
		{
			var member = _members.SingleOrDefault(i => i.Id == id);
			if (member == null) return NotFound();
			member.IsGuildMaster = true;
			return Ok(member);
		}

		[HttpPatch("{id}/demote", Name = "demote-member")]
		public IActionResult Demote(Guid id)
		{
			var member = _members.SingleOrDefault(i => i.Id == id);
			if (member == null) return NotFound();
			member.IsGuildMaster = false;
			return Ok(member);
		}

		[HttpPatch("{id}/leave", Name = "leave-guild")]
		public IActionResult LeaveGuild(Guid id)
		{
			var member = _members.SingleOrDefault(i => i.Id == id);
			if (member == null) return NotFound();
			member.Guild = null;
			member.GuildId = null;
			return Ok(member);
		}
	}
}