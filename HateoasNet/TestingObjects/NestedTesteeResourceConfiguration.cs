using HateoasNet.Abstractions;
using System.Linq;

namespace HateoasNet.TestingObjects
{
    public class NestedTesteeResourceConfiguration : IHateoasResourceConfiguration<NestedTestee>
    {
        public void Configure(IHateoasResource<NestedTestee> resource)
        {
            resource
                .HasLink("nested-test")
                .HasRouteData(t => new { id = t.Nested.StringValue })
                .HasConditional(t => t.Collection.Any());
        }
    }
}