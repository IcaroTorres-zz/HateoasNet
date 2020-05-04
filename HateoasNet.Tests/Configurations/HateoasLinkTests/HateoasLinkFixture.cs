using HateoasNet.Abstractions;
using HateoasNet.Configurations;
using HateoasNet.TestingObjects;
using System;

namespace HateoasNet.Tests.Configurations.HateoasLinkTests
{
    public class HateoasLinkFixture : IDisposable
    {
        public HateoasLinkFixture()
        {
            Sut = new HateoasLink<Testee>("test");
        }

        public IHateoasLink<Testee> Sut { get; private set; }

        public void Dispose()
        {
            Sut = null;
            GC.SuppressFinalize(this);
        }
    }
}
