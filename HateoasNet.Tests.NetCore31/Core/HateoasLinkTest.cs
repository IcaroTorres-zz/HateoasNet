using HateoasNet.Mapping;
using Sample.Models;
using Xunit;

namespace HateoasNet.Tests.NetCore31.Core
{
	public class HateoasLinkTest
	{
		[Fact]
		public void HasConditional__NotNullParameter_NotNullPredicateFunction()
		{
			// arrange
			const string routeName = "get-guild";
			var member = new Member();
			var hateoasMap = new HateoasMap<Member>();

			// act
			var hateoasLink = hateoasMap
				.HasLink(routeName)
				.HasRouteData(x => new {id = x.GuildId})
				.HasConditional(x => x.GuildId != null) as HateoasLink<Member>;

			var actualDisplayConditional = hateoasLink.PredicateFunction(member);
			var expectedDisplayConditional = hateoasLink.IsDisplayable(member);

			// assert
			Assert.NotNull(hateoasLink.PredicateFunction);
			Assert.Equal(expectedDisplayConditional, actualDisplayConditional);
		}

		[Fact]
		public void HasRouteData__NotNullParameter_NotNullRouteDataFunction()
		{
			// arrange
			const string routeName = "get-guild";
			var guild = new Guild();
			var hateoasMap = new HateoasMap<Guild>();

			// act
			var hateoasLink = hateoasMap
				.HasLink(routeName)
				.HasRouteData(x => new {id = x.Id}) as HateoasLink<Guild>;
			var actualRouteDictionary = hateoasLink.RouteDictionaryFunction(guild);
			var expectedRouteDictionary = hateoasLink.GetRouteDictionary(guild);

			// assert
			Assert.NotNull(hateoasLink.RouteDictionaryFunction);
			Assert.Equal(expectedRouteDictionary, actualRouteDictionary);
		}
	}
}