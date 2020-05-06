namespace HateoasNet.Resources
{
    /// <summary>
    ///   Represents a formatted wrapper of a single requested object in addition to HATEOAS <see cref="Resource.Links" />.
    /// </summary>
    [System.Serializable]
    public class SingleResource : Resource
    {
        public SingleResource(object data) : base(data)
        {
        }
    }
}
