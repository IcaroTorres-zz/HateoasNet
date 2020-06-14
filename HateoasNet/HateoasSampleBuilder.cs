using HateoasNet.Abstractions;
using System;

namespace HateoasNet.Tests.TestHelpers
{
    public class HateoasSampleBuilder : IHateoasSourceBuilder<HateoasSample>
    {
        public void Build(IHateoasSource<HateoasSample> source)
        {
            source.AddLink("test1").When(x => x.Id != null && x.Id != Guid.Empty).HasRouteData(x => new { id = x.Id });
            source.AddLink("test2").When(x => !string.IsNullOrWhiteSpace(x.ZipCode)).HasRouteData(x => new { zipcode = x.ZipCode });
        }
    }
}
