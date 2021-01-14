using System.Collections.Generic;

namespace HateoasNet
{
    /// <summary>
    ///   Represents a Factory for creating collections of <see cref="HateoasLink" /> output instances.
    /// </summary>
    public interface IHateoas
    {
        /// <summary>
        ///   Gets available collection of <see cref="HateoasLink"/> using <see cref="IHateoasSource{T}" /> configuration
        ///   for the <see cref="Type"/> of <paramref name="source" /> given as original value.
        /// </summary>
        /// <param name="source">Original value to look for mapped links.</param>
        /// <returns>A <see cref="IEnumerable{T}" /> of <see cref="HateoasLink"/> items.</returns>
        IEnumerable<HateoasLink> Generate(object source);
    }

    /// <summary>
    ///   Represents a Factory for creating customized <see cref="TOutput"/> from default generated collections of <see cref="HateoasLink" /> output instances.
    /// </summary>
    public interface IHateoas<out TOutput>
    {
        /// <summary>
        ///   Gets <typeparamref name="TOutput"/> value transformed from available collection of <see cref="HateoasLink"/> using <see cref="IHateoasSource{T}" />
        ///   configuration for the <see cref="Type"/> of <paramref name="source" /> given as original value.
        /// </summary>
        /// <param name="source">Original value to look for mapped links.</param>
        /// <returns>A custom <see cref="TOutput"/> created from <see cref="IEnumerable{T}" /> of <see cref="HateoasLink"/> ietms.</returns>
        TOutput Generate(object source);
    }
}
