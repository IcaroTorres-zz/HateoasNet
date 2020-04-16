using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HateoasNet.Abstractions;

namespace HateoasNet.Framework.Formatting
{
	public class HateoasMediaTypeFormatter : MediaTypeFormatter
	{
		private readonly IHateoasConfiguration _hateoasConfiguration;
		private readonly IHateoasSerializer _hateoasSerializer;
		private readonly IResourceFactory _resourceFactory;

		public HateoasMediaTypeFormatter(IHateoasConfiguration hateoasConfiguration, 
			IResourceFactory resourceFactory,
			IHateoasSerializer hateoasSerializer)
		{
			_hateoasConfiguration = hateoasConfiguration;
			_resourceFactory = resourceFactory;
			_hateoasSerializer = hateoasSerializer;
			
			SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json+hateoas"));

			SupportedEncodings.Add(new UTF8Encoding(false));
			SupportedEncodings.Add(Encoding.GetEncoding("iso-8859-1"));
		}

		public override bool CanReadType(Type type) => false;

		public override bool CanWriteType(Type type) => _hateoasConfiguration.HasMap(type);

		public override async Task WriteToStreamAsync(Type type,
			object value,
			Stream writeStream,
			HttpContent content,
			TransportContext transportContext,
			CancellationToken cancellationToken)
		{
			var requiredHeader = new KeyValuePair<string, IEnumerable<string>>("Accept", new[] {"application/json+hateoas"});
			var notSupportedMessage =
				$"The request using '{nameof(HateoasMediaTypeFormatter)}' does not have required Accept header '{SupportedMediaTypes.Last()}'.";

			if (!content.Headers.Contains(requiredHeader)) throw new NotSupportedException(notSupportedMessage);

			var effectiveEncoding = SelectCharacterEncoding(content.Headers);
			using var writer = new StreamWriter(writeStream, effectiveEncoding);
			var resource = value switch
			{
				IPagination pagination => _resourceFactory.Create(pagination, type),
				IEnumerable enumerable => _resourceFactory.Create(enumerable, type),
				_ => _resourceFactory.Create(value, type)
			};
			await writer.WriteAsync(_hateoasSerializer.SerializeResource(resource));
		}
	}
}
