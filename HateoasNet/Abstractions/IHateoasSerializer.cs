using HateoasNet.Resources;

namespace HateoasNet.Abstractions
{
    /// <summary>
    ///   Represents a custom Serializer for  <see cref="Resource" /> formatted with HATEOAS.
    /// </summary>
    public interface IHateoasSerializer
	{
        /// <summary>
        ///   Serializes a formatted instance inheriting from <see cref="Resource" />.
        /// </summary>
        /// <param name="resource">The formatted instance inheriting from <see cref="Resource" />.</param>
        /// <returns>Json <see langword="string" /> representing formatted <see cref="Resource" /> output.</returns>
        string SerializeResource(Resource resource);
	}
}
