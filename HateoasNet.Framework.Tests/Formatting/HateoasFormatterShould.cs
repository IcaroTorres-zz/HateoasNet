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
	public class HateoasMediaTypeFormatterShould
	{
		[Theory]
		[FormattingData]
		[Trait(nameof(HateoasMediaTypeFormatter), nameof(HateoasMediaTypeFormatter.WriteToStreamAsync))]
		public void Read_ExpectedText_FromResponseBody(object value, Resource resource, string text)
		{
			// arrange
			var type = value.GetType();
			var stream = new MemoryStream();
			var testHttpContent = new TestHttpContent();
			testHttpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json+hateoas");
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

			var sut = new HateoasMediaTypeFormatter(new Mock<IHateoasContext>().Object,
			                                        mockResourceFactory.Object,
			                                        new HateoasSerializer());

			// act
			sut.WriteToStreamAsync(type, value, stream, testHttpContent, null, CancellationToken.None).Wait();
			stream.Seek(0, SeekOrigin.Begin);
			mockResourceFactory.Verify();

			// read the stream to capture formatted string output
			var actual = new StreamReader(stream).ReadToEnd();

			// assert
			Assert.Equal(text, actual);
		}

		[Fact]
		[Trait(nameof(HateoasMediaTypeFormatter), nameof(HateoasMediaTypeFormatter.CanReadType))]
		public void ReturnsFalse_FromCalling_CanReadType_WithAnyValue()
		{
			var sut = new HateoasMediaTypeFormatter(new Mock<IHateoasContext>().Object,
			                                        new Mock<IResourceFactory>().Object,
			                                        new Mock<IHateoasSerializer>().Object);

			Assert.False(sut.CanReadType(It.IsAny<Type>()));
		}

		[Theory]
		[InlineData(typeof(Testee), typeof(NestedTestee))]
		[InlineData(typeof(NestedTestee), typeof(GenericTestee<Testee>))]
		[InlineData(typeof(GenericTestee<Testee>), typeof(Testee))]
		[Trait(nameof(HateoasMediaTypeFormatter), nameof(HateoasMediaTypeFormatter.CanWriteType))]
		public void ReturnsExpectedBool_FromCalling_CanReadType(Type validType, Type invalidType)
		{
			// arrange
			var mockContext = new Mock<IHateoasContext>();
			mockContext.Setup(x => x.HasResource(validType)).Returns(true);
			mockContext.Setup(x => x.HasResource(invalidType)).Returns(false);
			var sut = new HateoasMediaTypeFormatter(mockContext.Object,
			                                        new Mock<IResourceFactory>().Object,
			                                        new Mock<IHateoasSerializer>().Object);

			// act + asserts
			Assert.True(sut.CanWriteType(validType));
			Assert.False(sut.CanWriteType(invalidType));
		}
	}

	/// <summary>
	/// Class implementing abstract HttpContent for test purposes.
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
