#region Using

using C4rm4x.Tools.Security.Acl;
using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Security.Acl.Subscriptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Claims;
using System.Security.Principal;

#endregion

namespace C4rm4x.WebApi.Security.Acl.Test.Subscriptions
{
    public partial class SubscriberTest
    {
        [TestClass]
        public class SubscriberValidateCredentialsTest
        {
            private const string Secret = "5d41402abc4b2a76b9719d911017c592"; // hello

            [TestMethod, UnitTest]
            public void ValidateCredentials_Returns_False_When_Hash_Of_SharedSecret_Does_Not_Match_Secret()
            {
                IPrincipal principal;

                Assert.IsFalse(CreateSubjectUnderTest(Secret)
                    .ValidateCredentials(
                        new AclClientCredentials(ObjectMother.Create<string>(),  "hello!"),
                        out principal));
            }

            [TestMethod, UnitTest]
            public void ValidateCredentials_Returns_True_When_Hash_Of_SharedSecret_Matches_Secret()
            {
                IPrincipal principal;

                Assert.IsTrue(CreateSubjectUnderTest(Secret)
                    .ValidateCredentials(
                        new AclClientCredentials(ObjectMother.Create<string>(), "hello"),
                        out principal));
            }

            [TestMethod, UnitTest]
            public void ValidateCredentials_Sets_Principal_As_New_Instance_Of_ClaimsPrincipal_With_Basic_Authentication()
            {
                IPrincipal principal;
                var Identifier = ObjectMother.Create<string>();

                CreateSubjectUnderTest(Secret)
                    .ValidateCredentials(
                        new AclClientCredentials(Identifier, "hello"),
                        out principal);

                Assert.IsNotNull(principal);
                Assert.IsInstanceOfType(principal, typeof(ClaimsPrincipal));
                Assert.IsTrue(principal.Identity.IsAuthenticated);
            }

            private static Subscriber CreateSubjectUnderTest(
                string secret)
            {
                return new Subscriber
                {
                    Secret = secret,
                };
            }
        }
    }
}
