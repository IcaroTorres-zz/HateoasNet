using System.Collections.Generic;

namespace HateoasNet.Abstractions
{
    /// <summary>
    ///   Represents a Factory for creating collections of <see cref="HateoasLink" /> output instances.
    /// </summary>
    public interface IHateoas
    {
        /// <summary>
        ///   Gets available collection of <see cref="HateoasLink"/> using <see cref="IHateoasSource{T}" /> configuration
        ///   of <typeparamref name="T" /> with <paramref name="source" /> as original value.
        /// </summary>
        /// <param name="source">Original value to look for mapped links.</param>
        /// <typeparam name="T">type parameter of <see cref="IHateoasSource{T}" />  configuration.</typeparam>
        /// <returns>A <see cref="IEnumerable{T}" /> of <see cref="HateoasLink"/> ietms.</returns>
        IEnumerable<HateoasLink> Generate<T>(T source);
    }
}
