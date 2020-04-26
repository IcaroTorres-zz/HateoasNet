using System;
using HateoasNet.Abstractions;
using HateoasNet.Configurations;
using HateoasNet.TestingObjects;

namespace HateoasNet.Tests.Configurations.HateoasLinkTests
{
	public class HateoasLinkFixture : IDisposable
	{
		public HateoasLinkFixture()
		{
			Sut = new HateoasResource<Testee>().HasLink("test");
		}

		public IHateoasLink<Testee> Sut { get; private set; }

		public void Dispose()
		{
			Sut = null;
			GC.SuppressFinalize(this);
		}
	}
}
