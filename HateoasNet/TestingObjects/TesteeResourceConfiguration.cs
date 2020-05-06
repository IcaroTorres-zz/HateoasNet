using HateoasNet.Abstractions;

namespace HateoasNet.TestingObjects
{
	public class TesteeResourceConfiguration : IHateoasResourceConfiguration<Testee>
	{
		public void Configure(IHateoasResource<Testee> resource)
		{
			resource
				.HasLink("test")
				.HasRouteData(t => new {id = t.StringValue})
				.HasConditional(t => t.BoolValue);
		}
	}
}
