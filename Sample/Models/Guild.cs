using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sample.Models
{
	public class Guild
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public virtual string Name { get; set; }
		[JsonIgnore] public virtual ICollection<Member> Members { get; set; } = new List<Member>();
		[JsonIgnore] public virtual ICollection<Invite> Invites { get; set; } = new List<Invite>();


		public virtual void ChangeName(string newName)
		{
			Name = newName;
		}

		public virtual Invite Invite(Member member)
		{
			var invite = new Invite
			{
				Guild = this,
				GuildId = Id,
				Member = member, 
				MemberId = member.Id,
			};
			Invites.Add(invite);
			return invite;
		}

		public virtual Member AcceptMember(Member member)
		{
			Members.Add(member);
			if (Members.Count == 1) member.BePromoted();
			return member;
		}

		public virtual Member Promote(Member member)
		{
			return member.BePromoted();
		}

		public virtual Member KickMember(Member member)
		{
			if (member.IsGuildMaster) member.BeDemoted();
			Members.Remove(member);
			return member;
		}
	}
}