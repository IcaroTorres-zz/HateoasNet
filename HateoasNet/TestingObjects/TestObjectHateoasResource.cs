using HateoasNet.Abstractions;

namespace HateoasNet.TestingObjects
{
	public class TestObjectHateoasResource : IHateoasResourceConfiguration<TestObject>
	{
		public void Configure(IHateoasResource<TestObject> resource)
		{
			resource
				.HasLink("test")
				.HasRouteData(t => new {id = t.Value.ToString()})
				.HasConditional(t => t.Conditional);
		}
	}
}