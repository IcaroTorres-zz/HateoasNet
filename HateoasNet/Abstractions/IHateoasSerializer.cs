using HateoasNet.Resources;

namespace HateoasNet.Abstractions
{
  /// <summary>
  ///   Represents a custom Serializer for  <see cref="Resource" /> formatted with HATEOAS.
  /// </summary>
  public interface IHateoasSerializer
  {
    /// <summary>
    ///   Serializes a formatted <typeparamref name="TResource" /> instance.
    /// </summary>
    /// <param name="resource">The formatted <typeparamref name="TResource" /> inheriting from <see cref="Resource" />.</param>
    /// <typeparam name="TResource">The type inheriting from <see cref="Resource" />.<typeparam/>
    /// <returns>Json <see langword="string" /> representing formatted <typeparamref name="TResource" /> output.</returns>
    string SerializeResource<TResource>(TResource resource) where TResource : Resource;
  }
}