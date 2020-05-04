using System.Collections.Generic;

namespace HateoasNet.TestingObjects
{
    public class NestedTestee : Testee
    {
        public Testee Nested { get; set; } = new Testee();
        public ICollection<Testee> Collection { get; set; } = new List<Testee>() { new Testee() };
    }
}
