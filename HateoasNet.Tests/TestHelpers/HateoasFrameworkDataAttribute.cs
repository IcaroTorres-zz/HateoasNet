using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace HateoasNet.Tests.TestHelpers
{
    public class HateoasFrameworkDataAttribute : DataAttribute
    {
        /// <inheritdoc />
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { new HateoasTestData("create-order", "order", "POST") };

            var getValues = new Dictionary<string, object> { { "typeId", Guid.NewGuid() } };
            yield return new object[] { new HateoasTestData("get-types", "types", "GET", getValues) };

            var putValues = new Dictionary<string, object> { { "userName", "john doe" } };
            yield return new object[] { new HateoasTestData("update-user", "users", "PUT", putValues) };

            var patchValues = new Dictionary<string, object> { { "inviteId", 21 } };
            yield return new object[] { new HateoasTestData("accept-invite", "invites", "PATCH", patchValues) };

            var deleteValues = new Dictionary<string, object> { { "friendId", Guid.NewGuid() } };
            yield return new object[] { new HateoasTestData("remove-friend", "friends", "DELETE", deleteValues) };
        }
    }
}
