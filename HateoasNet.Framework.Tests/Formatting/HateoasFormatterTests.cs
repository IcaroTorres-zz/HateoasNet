using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using HateoasNet.Abstractions;
using HateoasNet.Framework.Formatting;
using HateoasNet.Framework.Serialization;
using HateoasNet.Resources;
using HateoasNet.TestingObjects;
using Moq;
using Xunit;

namespace HateoasNet.Framework.Tests.Formatting
{
	public class HateoasMediaTypeFormatterTests : IDisposable
	{
		/// <inheritdoc />
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		[Theory]
		[FormattingData]
		[Trait(nameof(HateoasMediaTypeFormatter), nameof(HateoasMediaTypeFormatter.WriteToStreamAsync))]
		public void WriteToStreamAsync_ValidParameters_WriteExpectedText(object value, Resource resource, string text)
		{
			// arrange
			const string supportedContentType = "application/json+hateoas";
			var type = value.GetType();
			var stream = new MemoryStream();
			var testHttpContent = new TestHttpContent();
			testHttpContent.Headers.ContentType = new MediaTypeHeaderValue(supportedContentType);
			var mockResourceFactory = GenerateFullResourceFactoryMock(resource, value, type);
			var sut = new HateoasMediaTypeFormatter(new Mock<IHateoasContext>().Object,
			                                        mockResourceFactory.Object,
			                                        new HateoasSerializer());

			// act
			sut.WriteToStreamAsync(type, value, stream, testHttpContent, null, CancellationToken.None).Wait();
			stream.Seek(0, SeekOrigin.Begin);
			mockResourceFactory.Verify();
			// reset position and read the stream to capture formatted string output
			var actual = new StreamReader(stream).ReadToEnd();

			// assert
			Assert.Equal(text, actual);
		}

		[Theory]
		[InlineData(typeof(Testee), typeof(NestedTestee))]
		[InlineData(typeof(NestedTestee), typeof(GenericTestee<Testee>))]
		[InlineData(typeof(GenericTestee<Testee>), typeof(Testee))]
		[Trait(nameof(HateoasMediaTypeFormatter), nameof(HateoasMediaTypeFormatter.CanWriteType))]
		public void CanWriteType_WithTypeParameters_ReturnsExpectedBool(Type validType, Type invalidType)
		{
			// arrange
			var hateoasContextMock = GenerateCanWriteHateoasContextMock(validType, invalidType);
			var sut = new HateoasMediaTypeFormatter(hateoasContextMock.Object,
			                                        new Mock<IHateoasFactory>().Object,
			                                        new Mock<IHateoasSerializer>().Object);

			// act
			var actualValid = sut.CanWriteType(validType);
			var actualInvalid = sut.CanWriteType(invalidType);

			// assert
			Assert.True(actualValid);
			Assert.False(actualInvalid);
		}

		private Mock<IHateoasFactory> GenerateFullResourceFactoryMock(Resource resource, object value, Type type)
		{
			var mockResourceFactory = new Mock<IHateoasFactory>();
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

		private Mock<IHateoasContext> GenerateCanWriteHateoasContextMock(Type validType, Type invalidType)
		{
			var mockContext = new Mock<IHateoasContext>();
			mockContext.Setup(x => x.HasResource(validType)).Returns(true);
			mockContext.Setup(x => x.HasResource(invalidType)).Returns(false);

			return mockContext;
		}

		[Fact]
		[Trait(nameof(HateoasMediaTypeFormatter), nameof(HateoasMediaTypeFormatter.CanReadType))]
		public void CanReadType_Always_ReturnsFalse()
		{
			// arrange
			var sut = new HateoasMediaTypeFormatter(new Mock<IHateoasContext>().Object,
			                                        new Mock<IHateoasFactory>().Object,
			                                        new Mock<IHateoasSerializer>().Object);

			// act
			var actual = sut.CanReadType(It.IsAny<Type>());

			// assert
			Assert.False(actual);
		}
	}

    /// <summary>
    ///   Class implementing abstract HttpContent for test purposes.
    /// </summary>
    public class TestHttpContent : HttpContent
	{
		/// <inheritdoc />
		protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
		{
			await Task.FromResult<object>(null);
		}

		/// <inheritdoc />
		protected override bool TryComputeLength(out long length)
		{
			length = 0;
			return false;
		}
	}
}
