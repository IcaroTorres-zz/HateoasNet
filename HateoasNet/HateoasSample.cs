using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace HateoasNet
{
    [ExcludeFromCodeCoverage]
    public class HateoasSample
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ForeignKeyId { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = "sample_policy_owner@samplehateoas.com";
        public string ProcessNumber { get; set; } = "0002080-25.2012.5.15.0049";
        public string DocumentNumber { get; set; } = "02903084530";
        public string FileName { get; set; } = "sample_document_name.pdf";
        public string ZipCode { get; set; } = "49040700";

        public IEnumerable<HateoasLink> HateoasLinks { get; set; }
    }
}
