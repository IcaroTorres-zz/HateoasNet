using HateoasNet.Resources;

namespace HateoasNet.Abstractions
{
	/// <summary>
	/// Represents a custom Serializer for  <see cref="Resource"/> formatted with HATEOAS.
	/// </summary>
	public interface IHateoasSerializer
	{
		/// <summary>
		/// Serialize a formatted <see cref="Resource"/> instance.
		/// </summary>
		/// <param name="resource">The formatted <see cref="Resource"/>.</param>
		/// <returns>Json <see cref="string"/> representing formatted <see cref="Resource"/> output.</returns>
		string SerializeResource(Resource resource);
	}
}