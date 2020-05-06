using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace HateoasNet.Framework.Tests.Factories
{
    /// <summary>
    /// 	Custom dummy of HttpActionDescriptor abstract class to get RouteAttribute
    /// 	and HttpMethod from a Controller Action MethodInfo for testing purposes
    /// </summary>
    internal class TestActionDescriptor : HttpActionDescriptor
    {
        private readonly Collection<HttpParameterDescriptor> _parameterDescriptors;

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public MethodInfo MethodInfo { get; }

        public TestActionDescriptor(HttpControllerDescriptor controllerDescriptor,
                                    MethodInfo methodInfo,
                                    Collection<HttpParameterDescriptor> parameterDescriptors) : base(controllerDescriptor)
        {
            _parameterDescriptors = parameterDescriptors;
            MethodInfo = methodInfo;
        }

        /// <inheritdoc />
        public override Collection<HttpParameterDescriptor> GetParameters() => _parameterDescriptors;

        /// <inheritdoc />
        public override Collection<T> GetCustomAttributes<T>() => new Collection<T>();

        /// <inheritdoc />
        public override Task<object> ExecuteAsync(HttpControllerContext controllerContext,
                                                  IDictionary<string, object> arguments,
                                                  CancellationToken cancellationToken) => null;

        /// <inheritdoc />
        public override string ActionName => "TestAction";

        /// <inheritdoc />
        public override Type ReturnType => typeof(object);
    }
}
