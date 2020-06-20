using HateoasNet.Abstractions;
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
        ///   of <typeparamref name="T" /> with <paramref name="source" /> as original value.
        /// </summary>
        /// <param name="source">Original value to look for mapped links.</param>
        /// <typeparam name="T">type parameter of <see cref="IHateoasSource{T}" />  configuration.</typeparam>
        /// <returns>A <see cref="IEnumerable{T}" /> of <see cref="HateoasLink"/> ietms.</returns>
        IEnumerable<HateoasLink> Generate<T>(T source);
    }

    /// <summary>
    ///   Represents a Factory for creating customized <see cref="TOutput"/> from default generated collections of <see cref="HateoasLink" /> output instances.
    /// </summary>
    public interface IHateoas<TOutput>
    {
        /// <summary>
        ///   Gets <typeparamref name="TOutput"/> value transformed from available collection of <see cref="HateoasLink"/> using <see cref="IHateoasSource{T}" />
        ///   configuration of <typeparamref name="TSource" /> with <paramref name="source" /> as original value.
        /// </summary>
        /// <param name="source">Original value to look for mapped links.</param>
        /// <typeparam name="TSource">type parameter of <see cref="IHateoasSource{T}" />  configuration.</typeparam>
        /// <returns>A custom <see cref="TOutput"/> created from <see cref="IEnumerable{T}" /> of <see cref="HateoasLink"/> ietms.</returns>
        TOutput Generate<TSource>(TSource source);
    }
}
