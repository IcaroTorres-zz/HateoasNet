using HateoasNet.Abstractions;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace HateoasNet.Core.Sample.CustomHateoas
{
    public class DictionaryHateoas : AbstractHateoas<ImmutableDictionary<string, object>>
    {
        public DictionaryHateoas(IHateoas hateoas) : base(hateoas)
        {
        }

        public override ImmutableDictionary<string, object> GenerateCustom(IEnumerable<HateoasLink> links)
        {
            return links.ToImmutableDictionary(x => x.Rel, x => (object)new { x.Href, x.Method });
        }
    }
}
