using HateoasNet.Framework.Sample.JsonData;
using HateoasNet.Framework.Sample.Models;
using HateoasNet.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace HateoasNet.Framework.Sample.Controllers
{
    [RoutePrefix("some-other-prefix/invites")]
    public class InvitesController : ApiController
    {
        private readonly List<Invite> _invites;

        public InvitesController(Seeder seeder)
        {
            _invites = seeder.Seed<Invite>();
        }

        [HttpGet]
        [Route("{id:Guid}", Name = "get-invite")]
        public IHttpActionResult Get(Guid id)
        {
            var invite = _invites.SingleOrDefault(i => i.Id == id);
            return invite != null ? Ok(invite) : NotFound() as IHttpActionResult;
        }

        [HttpGet]
        [Route(Name = "get-invites")]
        public IHttpActionResult Get(int pageSize = 5)
        {
            return Ok(new Pagination<Invite>(_invites.Take(pageSize), _invites.Count, pageSize));
        }

        [HttpPost]
        [Route(Name = "invite-member")]
        public IHttpActionResult Post([FromBody] Invite invite)
        {
            return CreatedAtRoute("get-invite", new { id = invite.Id }, invite);
        }

        [HttpPatch]
        [Route("{id}/accept", Name = "accept-invite")]
        public IHttpActionResult Accept(Guid id)
        {
            var invite = _invites.SingleOrDefault(i => i.Id == id);
            if (invite == null) return NotFound();
            invite.Status = InviteStatuses.Accepted;
            return Ok(invite);
        }

        [HttpPatch]
        [Route("{id}/decline", Name = "decline-invite")]
        public IHttpActionResult Decline(Guid id)
        {
            var invite = _invites.SingleOrDefault(i => i.Id == id);
            if (invite == null) return NotFound();
            invite.Status = InviteStatuses.Declined;
            return Ok(invite);
        }

        [HttpPatch]
        [Route("{id}/cancel", Name = "cancel-invite")]
        public IHttpActionResult Cancel(Guid id)
        {
            var invite = _invites.SingleOrDefault(i => i.Id == id);
            if (invite == null) return NotFound();
            invite.Status = InviteStatuses.Canceled;
            return Ok(invite);
        }
    }
}
