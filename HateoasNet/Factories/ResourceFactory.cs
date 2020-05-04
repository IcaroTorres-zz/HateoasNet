using HateoasNet.Abstractions;
using HateoasNet.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HateoasNet.Factories
{
    /// <inheritdoc cref="IResourceFactory" />
    public sealed class ResourceFactory : IResourceFactory
    {
        private readonly IHateoasContext _hateoasConfiguration;
        private readonly IResourceLinkFactory _resourceLinkFactory;

        public ResourceFactory(IHateoasContext hateoasConfiguration, IResourceLinkFactory resourceLinkFactory)
        {
            _hateoasConfiguration = hateoasConfiguration;
            _resourceLinkFactory = resourceLinkFactory;
        }

        public SingleResource Create(object source, Type type)
        {
            var singleResource = new SingleResource(source);
            BuildResourceLinks(singleResource, source, type);
            return singleResource;
        }

        public EnumerableResource Create(IEnumerable source, Type type)
        {
            var enumerableResource = new EnumerableResource(ToEnumerableOfResources(source, type));
            BuildResourceLinks(enumerableResource, source, type);
            return enumerableResource;
        }

        public PaginationResource Create(IPagination source, Type type)
        {
            var singleResources = ToEnumerableOfResources(source.GetEnumeration(), type);
            var paginationResource = new PaginationResource(singleResources, source.Count, source.PageSize, source.Page);
            BuildResourceLinks(paginationResource, source, type);
            return paginationResource;
        }

        /// <summary>
        ///   Builds the <see cref="Resource.Links" /> collection for created <see cref="Resource" />.
        /// </summary>
        /// <param name="resource">Original value wrapped in a <see cref="Resource" /> instance.</param>
        /// <param name="value">Original value of the configured type.</param>
        /// <param name="type">
        ///   type parameter of <see cref="IHateoasLink{T}" /> configuration to builds the <see cref="Resource.Links" />.
        /// </param>
        internal void BuildResourceLinks(Resource resource, object data, Type type)
        {
            foreach (var hateoasLink in _hateoasConfiguration.GetApplicableLinks(type, data))
            {
                var createdLink =
                    _resourceLinkFactory.Create(hateoasLink.RouteName, hateoasLink.GetRouteDictionary(data));

                resource.Links.Add(createdLink);
            }
        }

        internal IEnumerable<Resource> ToEnumerableOfResources(IEnumerable source, Type type)
        {
            return from object item in source select Create(item, type.GetGenericArguments().First());
        }
    }
}
