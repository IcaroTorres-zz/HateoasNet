using System;
using Newtonsoft.Json;

namespace HateoasNet.Framework.Sample.Models
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
