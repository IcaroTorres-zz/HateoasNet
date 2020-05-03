using System.Collections;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HateoasNet.Abstractions;
using HateoasNet.Core.Formatting;
using HateoasNet.Core.Serialization;
using HateoasNet.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Moq;
using Xunit;

namespace HateoasNet.Core.Tests.Formatting
{
	public class HateoasFormatterShould
	{
		[Theory]
		[FormattingData]
		[Trait(nameof(HateoasFormatter), nameof(HateoasFormatter.WriteResponseBodyAsync))]
		public async Task Read_ExpectedText_FromResponseBody(object value, Resource resource, string text)
		{
			// arrange
			var type = value.GetType();

			var httpContext = new DefaultHttpContext();
			httpContext.Response.Body = new MemoryStream();

			var context = new OutputFormatterWriteContext(httpContext, (s, e) => new StreamWriter(s, e), type, value);

			var mockResourceFactory = new Mock<IResourceFactory>();

			switch (value)
			{
				case IEnumerable enumerable:
					mockResourceFactory.Setup(x => x.Create(enumerable, type))
					                   .Returns(resource as EnumerableResource)
					                   .Verifiable("unable to call Create with enumerable.");
					break;
				case IPagination pagination:
					mockResourceFactory.Setup(x => x.Create(pagination, type))
					                   .Returns(resource as PaginationResource)
					                   .Verifiable("unable to call Create with pagination.");
					break;
				default:
					mockResourceFactory.Setup(x => x.Create(value, type))
					                   .Returns(resource as SingleResource)
					                   .Verifiable("unable to call Create with object.");
					break;
			}

			var sut = new HateoasFormatter(mockResourceFactory.Object, new HateoasSerializer());

			// act
			await sut.WriteResponseBodyAsync(context);
			mockResourceFactory.Verify();

			// read the stream to capture formatted string output
			context.HttpContext.Response.Body.Seek(0, SeekOrigin.Begin);
			var actual = await new StreamReader(context.HttpContext.Response.Body).ReadToEndAsync();

			// assert
			Assert.Equal(sut.SupportedMediaTypes.Last(), context.HttpContext.Response.ContentType);
			Assert.Equal(text, actual);
		}
	}
}
