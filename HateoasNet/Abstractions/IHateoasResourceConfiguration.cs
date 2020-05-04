namespace HateoasNet.Abstractions
{
    /// <summary>
    ///   Represents Configuration for HATEOAS resource targeting <typeparamref name="T" /> in separated class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHateoasResourceConfiguration<T> where T : class
    {
        /// <summary>
        ///   Configure resource using implemented logic.
        /// </summary>
        /// <param name="resource">An <see cref="IHateoasResource{T}" /> instance to configure, generated internally.</param>
        void Configure(IHateoasResource<T> resource);
    }
}