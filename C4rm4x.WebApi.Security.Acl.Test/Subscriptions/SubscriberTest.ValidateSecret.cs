#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Security.Acl.Subscriptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.Security.Acl.Test.Subscriptions
{
    public partial class SubscriberTest
    {
        [TestClass]
        public class SubscriberValidateSecretTest
        {
            private const string Secret = "5d41402abc4b2a76b9719d911017c592"; // hello

            [TestMethod, UnitTest]
            public void ValidateSecret_Returns_False_When_Hash_Of_SharedSecret_Does_Not_Match_Secret()
            {
                Assert.IsFalse(CreateSubjectUnderTest(Secret)
                    .ValidateSecret("hello!"));
            }

            [TestMethod, UnitTest]
            public void ValidateSecret_Returns_True_When_Hash_Of_SharedSecret_Matches_Secret()
            {
                Assert.IsTrue(CreateSubjectUnderTest(Secret)
                    .ValidateSecret("hello"));
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
