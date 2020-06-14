using HateoasNet.Abstractions;
using HateoasNet.Framework.Sample.JsonData;
using HateoasNet.Framework.Sample.Models;
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
        private readonly IHateoas _hateoas;

        public InvitesController(Seeder seeder, IHateoas hateoas)
        {
            _invites = seeder.Seed<Invite>();
            _hateoas = hateoas;
        }

        [HttpGet, Route("{id:guid}", Name = "get-invite")]
        public IHttpActionResult Get(Guid id)
        {
            var invite = _invites.SingleOrDefault(i => i.Id == id);
            var links = _hateoas.Generate(invite);
            return invite != null ? Ok(new { data = invite, links }) : NotFound() as IHttpActionResult;
        }

        [HttpGet, Route(Name = "get-invites")]
        public IHttpActionResult Get(int pageSize = 5)
        {
            var invites = _invites.Take(pageSize).ToList();
            var links = _hateoas.Generate(invites);
            return Ok(new { data = invites, links });
        }

        [HttpPost, Route(Name = "invite-member")]
        public IHttpActionResult Post([FromBody] Invite invite)
        {
            var links = _hateoas.Generate(invite);
            return CreatedAtRoute("get-invite", new { id = invite.Id }, new { data = invite, links });
        }

        [HttpPatch, Route("{id}/accept", Name = "accept-invite")]
        public IHttpActionResult Accept(Guid id)
        {
            var invite = _invites.SingleOrDefault(i => i.Id == id);
            if (invite == null) return NotFound();
            invite.Status = InviteStatuses.Accepted;
            var links = _hateoas.Generate(invite);
            return Ok(new { data = invite, links });
        }

        [HttpPatch, Route("{id}/decline", Name = "decline-invite")]
        public IHttpActionResult Decline(Guid id)
        {
            var invite = _invites.SingleOrDefault(i => i.Id == id);
            if (invite == null) return NotFound();
            invite.Status = InviteStatuses.Declined;
            var links = _hateoas.Generate(invite);
            return Ok(new { data = invite, links });
        }

        [HttpPatch, Route("{id}/cancel", Name = "cancel-invite")]
        public IHttpActionResult Cancel(Guid id)
        {
            var invite = _invites.SingleOrDefault(i => i.Id == id);
            if (invite == null) return NotFound();
            invite.Status = InviteStatuses.Canceled;
            var links = _hateoas.Generate(invite);
            return Ok(new { data = invite, links });
        }
    }
}
