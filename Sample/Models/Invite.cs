using System;
using System.Text.Json.Serialization;

namespace Sample.Models
{
	public class Invite
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public virtual InviteStatuses Status { get; set; } = InviteStatuses.Pending;
		public Guid GuildId { get; set; }
		public Guid MemberId { get; set; }
		[JsonIgnore] public virtual Member Member { get; set; }
		[JsonIgnore] public virtual Guild Guild { get; set; }

		public virtual Invite BeAccepted()
		{
			Status = InviteStatuses.Accepted;
			Guild.AcceptMember(Member.JoinGuild(Guild));
			return this;
		}

		public virtual Invite BeDeclined()
		{
			Status = InviteStatuses.Declined;
			return this;
		}

		public virtual Invite BeCanceled()
		{
			Status = InviteStatuses.Canceled;
			return this;
		}
	}

	public enum InviteStatuses : short
	{
		/// <summary>
		///   Waiting for an answer as accepted, declined or canceled
		/// </summary>
		Pending = 1,

		/// <summary>
		///   Accepted by invited member
		/// </summary>
		Accepted,

		/// <summary>
		///   Declined by invited member
		/// </summary>
		Declined,

		/// <summary>
		///   Canceled by inviting guild
		/// </summary>
		Canceled
	}
}