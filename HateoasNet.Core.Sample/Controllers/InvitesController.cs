using HateoasNet.Core.Sample.JsonData;
using HateoasNet.Core.Sample.Models;
using HateoasNet.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HateoasNet.Core.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvitesController : ControllerBase
    {
        private readonly List<Invite> _invites;

        public InvitesController(Seeder seeder)
        {
            _invites = seeder.Seed<Invite>();
        }

        [HttpGet("{id:Guid}", Name = "get-invite")]
        public IActionResult Get(Guid id)
        {
            var invite = _invites.SingleOrDefault(i => i.Id == id);
            return invite != null ? Ok(invite) : NotFound() as IActionResult;
        }

        [HttpGet(Name = "get-invites")]
        public IActionResult Get([FromQuery(Name = "pageSize")] int pageSize = 5)
        {
            return Ok(new Pagination<Invite>(_invites.Take(pageSize), _invites.Count, pageSize));
        }

        [HttpPost(Name = "invite-member")]
        public IActionResult Post([FromBody] Invite invite)
        {
            return CreatedAtRoute("get-invite", new { id = invite.Id }, invite);
        }

        [HttpPatch("{id}/accept", Name = "accept-invite")]
        public IActionResult Accept(Guid id)
        {
            var invite = _invites.SingleOrDefault(i => i.Id == id);
            if (invite == null) return NotFound();
            invite.Status = InviteStatuses.Accepted;
            return Ok(invite);
        }

        [HttpPatch("{id}/decline", Name = "decline-invite")]
        public IActionResult Decline(Guid id)
        {
            var invite = _invites.SingleOrDefault(i => i.Id == id);
            if (invite == null) return NotFound();
            invite.Status = InviteStatuses.Declined;
            return Ok(invite);
        }

        [HttpPatch("{id}/cancel", Name = "cancel-invite")]
        public IActionResult Cancel(Guid id)
        {
            var invite = _invites.SingleOrDefault(i => i.Id == id);
            if (invite == null) return NotFound();
            invite.Status = InviteStatuses.Canceled;
            return Ok(invite);
        }
    }
}