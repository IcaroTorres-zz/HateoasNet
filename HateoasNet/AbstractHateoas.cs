using HateoasNet.Abstractions;
using System.Collections.Generic;

namespace HateoasNet
{
    /// <inheritdoc cref="IHateoas{TOutput}" />
    public abstract class AbstractHateoas<TOutput> : IHateoas<TOutput>
    {
        private readonly IHateoas _hateoas;

        public AbstractHateoas(IHateoas hateoas)
        {
            _hateoas = hateoas;
        }

        public TOutput Generate<TSource>(TSource source)
        {
            return GenerateCustom(_hateoas.Generate(source));
        }

        public abstract TOutput GenerateCustom(IEnumerable<HateoasLink> links);
    }
}
