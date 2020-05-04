using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HateoasNet.Core.Sample.Models
{
    public class Guild
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        [JsonIgnore] public ICollection<Member> Members { get; set; } = new List<Member>();
        [JsonIgnore] public ICollection<Invite> Invites { get; set; } = new List<Invite>();
    }
}