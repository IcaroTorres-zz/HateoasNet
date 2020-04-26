using HateoasNet.Abstractions;
using Xunit;

namespace HateoasNet.Tests.Configurations.HateoasLinkTests
{
	[CollectionDefinition(nameof(IHateoasLink))]
	public class HateoasLinkCollectionFixture : ICollectionFixture<HateoasLinkFixture>
	{
	}
}