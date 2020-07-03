using System.Diagnostics.CodeAnalysis;

namespace HateoasNet
{
    /// <summary>
    ///   Represents an HATEOAS link holding a <see cref="Rel" /> expressing action intent,
    ///   <see cref="Href" /> as the action URL and <see cref="Method" /> being accepted HTTP method.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class HateoasLink
    {
        public HateoasLink(string rel, string href, string method)
        {
            Rel = rel;
            Href = href;
            Method = method;
        }

        /// <summary>
        ///   The Url to access some available action of the subject source containing this <see cref="HateoasLink" />.
        /// </summary>
        /// <value>Url <see langword="string" /> value.</value>
        public string Href { get; }

        /// <summary>
        ///   The name assigned to the action available in the subject source containing this <see cref="HateoasLink" />.
        /// </summary>
        /// <value>The <see langword="string" /> value of assigned <see cref="Rel" />.</value>
        public string Rel { get; }

        /// <summary>
        ///   The HTTP method used to access the action available in the subject source containing this <see cref="HateoasLink" />.
        /// </summary>
        /// <value>The <see langword="string" /> value of HTTP method.</value>
        public string Method { get; }
    }
}
