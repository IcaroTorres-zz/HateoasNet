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
	public class InvitesController : ControllerBase
	{
		private readonly List<Invite> _invites;
		private readonly IHateoas _hateoas;

		public InvitesController(Seeder seeder, IHateoas hateoas)
		{
			_invites = seeder.Seed<Invite>();
			_hateoas = hateoas;
		}

		[HttpGet("{id:Guid}", Name = "get-invite")]
		public IActionResult Get(Guid id)
		{
			var invite = _invites.SingleOrDefault(i => i.Id == id);
			var links = _hateoas.Generate(invite);
			return invite != null ? Ok(new { data = invite, links }) : NotFound() as IActionResult;
		}

		[HttpGet(Name = "get-invites")]
		public IActionResult Get([FromQuery(Name = "pageSize")] int pageSize = 5)
		{
			var invites = _invites.Take(pageSize);
			var links = _hateoas.Generate(invites);
			return Ok(new { data = invites, links });
		}

		[HttpPost(Name = "invite-member")]
		public IActionResult Post([FromBody] Invite invite)
		{
			var links = _hateoas.Generate(invite);
			return CreatedAtRoute("get-invite", new {id = invite.Id}, new { data = invite, links });
		}

		[HttpPatch("{id}/accept", Name = "accept-invite")]
		public IActionResult Accept(Guid id)
		{
			var invite = _invites.SingleOrDefault(i => i.Id == id);
			if (invite == null) return NotFound();
			invite.Status = InviteStatuses.Accepted;
			var links = _hateoas.Generate(invite);
			return Ok(new { data = invite, links });
		}

		[HttpPatch("{id}/decline", Name = "decline-invite")]
		public IActionResult Decline(Guid id)
		{
			var invite = _invites.SingleOrDefault(i => i.Id == id);
			if (invite == null) return NotFound();
			invite.Status = InviteStatuses.Declined;
			var links = _hateoas.Generate(invite);
			return Ok(new { data = invite, links });
		}

		[HttpPatch("{id}/cancel", Name = "cancel-invite")]
		public IActionResult Cancel(Guid id)
		{
			var invite = _invites.SingleOrDefault(i => i.Id == id);
			if (invite == null) return NotFound();
			invite.Status = InviteStatuses.Canceled;
			var links = _hateoas.Generate(invite);
			return Ok(new { data = invite, links });
		}
	}
}
