using HateoasNet.Abstractions;

namespace HateoasNet.TestingObjects
{
	public class TestObjectHateoas : IHateoasBuilder<TestObject>
	{
		public void Build(IHateoasMap<TestObject> map)
		{
			map
				.HasLink("test")
				.HasRouteData(t => new {id = t.Value.ToString()})
				.HasConditional(t => t.Conditional);
		}
	}
}