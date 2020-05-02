using System.Collections.Generic;

namespace HateoasNet.TestingObjects
{
  public class GenericTestee<T> : Testee
  {
    public T Nested { get; set; }
    public List<T> Collection { get; set; } = new List<T>();
  }
}