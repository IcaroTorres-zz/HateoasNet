using System.Linq;
using HateoasNet.Abstractions;

namespace HateoasNet.TestingObjects
{
	public class GenericTesteeResourceConfiguration : IHateoasResourceConfiguration<GenericTestee<object>>
	{
		public void Configure(IHateoasResource<GenericTestee<object>> resource)
		{
			resource
				.HasLink("generic-test")
				.HasRouteData(t => new {id = t.Nested.ToString()})
				.HasConditional(t => t.Collection.OfType<Testee>().Any());
		}
	}
}