#if NET472
using System;
using System.Globalization;
using System.Reflection;
using System.Web.Http;

namespace HateoasNet.Tests.TestHelpers
{
    /// <summary>
    ///   Custom dummy of MethodInfo abstract class to get RouteAttribute values from a
    ///   Controller Action MethodInfo for testing purposes
    /// </summary>
    internal class TestMethodInfo : MethodInfo
    {
        private readonly RouteAttribute _routeAttribute;

        public TestMethodInfo(RouteAttribute routeAttribute)
        {
            _routeAttribute = routeAttribute;
        }

        /// <inheritdoc />
        public override ICustomAttributeProvider ReturnTypeCustomAttributes { get; }

        /// <inheritdoc />
        public override string Name => "TestAction";

        /// <inheritdoc />
        public override Type DeclaringType => typeof(object);

        /// <inheritdoc />
        public override Type ReflectedType => typeof(object);

        /// <inheritdoc />
        public override RuntimeMethodHandle MethodHandle => new RuntimeMethodHandle();

        /// <inheritdoc />
        public override MethodAttributes Attributes => new MethodAttributes();

        /// <inheritdoc />
        public override object[] GetCustomAttributes(bool inherit)
        {
            return new object[] { _routeAttribute };
        }

        /// <inheritdoc />
        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return false;
        }

        /// <inheritdoc />
        public override ParameterInfo[] GetParameters()
        {
            return new ParameterInfo[] { };
        }

        /// <inheritdoc />
        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            return MethodImplAttributes.IL;
        }

        /// <inheritdoc />
        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            return null;
        }

        /// <inheritdoc />
        public override MethodInfo GetBaseDefinition()
        {
            return null;
        }

        /// <inheritdoc />
        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return new object[] { _routeAttribute };
        }
    }
}
#endif
