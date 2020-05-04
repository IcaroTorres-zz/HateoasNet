using System;
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
	public class HateoasFormatterTests : IDisposable
	{
		[Theory]
		[FormattingData]
		[Trait(nameof(HateoasFormatter), nameof(HateoasFormatter.WriteResponseBodyAsync))]
		public async Task WriteResponseBodyAsync_ValidParameters_WriteExpectedText(
			object value, Resource resource, string text)
		{
			// arrange
			var type = value.GetType();
			var mockResourceFactory = GenerateFullResourceFactoryMock(resource, value, type);
			var sut = new HateoasFormatter(mockResourceFactory.Object, new HateoasSerializer());
			var expectedContentType = sut.SupportedMediaTypes.Last();
			var context = GenerateParameterOutputFormatterWriteContext(value, type);

			// act
			await sut.WriteResponseBodyAsync(context);
			mockResourceFactory.Verify();
			// reset position and read the stream to capture formatted string output
			context.HttpContext.Response.Body.Seek(0, SeekOrigin.Begin);
			var actualOutput = await new StreamReader(context.HttpContext.Response.Body).ReadToEndAsync();
			var actualContentType = context.HttpContext.Response.ContentType;

			// assert
			Assert.Equal(expectedContentType, actualContentType);
			Assert.Equal(text, actualOutput);
		}

		private Mock<IResourceFactory> GenerateFullResourceFactoryMock(Resource resource, object value, Type type)
		{
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

			return mockResourceFactory;
		}

		private OutputFormatterWriteContext GenerateParameterOutputFormatterWriteContext(object value, Type type)
		{
			var httpContext = new DefaultHttpContext();
			httpContext.Response.Body = new MemoryStream();
			return new OutputFormatterWriteContext(httpContext, (s, e) => new StreamWriter(s, e), type, value);
		}

		/// <inheritdoc />
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}
