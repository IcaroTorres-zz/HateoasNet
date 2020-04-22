using System;
using System.Text.Json.Serialization;

namespace HateoasNet.Core.Sample.Models
{
	public class Member
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string Name { get; set; }
		public bool IsGuildMaster { get; set; }
		public Guid? GuildId { get; set; }
		[JsonIgnore] public Guild Guild { get; set; }
	}
}