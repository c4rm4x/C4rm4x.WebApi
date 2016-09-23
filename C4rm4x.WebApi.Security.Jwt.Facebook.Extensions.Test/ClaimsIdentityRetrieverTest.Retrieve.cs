#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Security.Jwt.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Security.Claims;

#endregion

namespace C4rm4x.WebApi.Security.Jwt.Facebook.Extensions.Test
{
    public partial class ClaimsIdentityRetrieverTest
    {
        #region Helper classes

        public class TestClaimsIdentityRetriever :
            ClaimsIdentityRetriever
        {
            public ClaimsIdentity ClaimsIdentity { get; private set; }

            public TestClaimsIdentityRetriever(
                ClaimsIdentity claimsIdentity)
            {
                claimsIdentity.NotNull(nameof(claimsIdentity));

                ClaimsIdentity = claimsIdentity;
            }

            protected override ClaimsIdentity Retrieve(FBUser user)
            {
                return ClaimsIdentity;
            }
        }

        #endregion

        [TestClass]
        public class ClaimsIdentityRetrieverRetrieverTest
        {
            [TestMethod, UnitTest]
            [ExpectedException(typeof(UserCredentialsException))]
            public void Retrieve_Throws_UserCredentialsException_When_Secret_Is_Not_A_Valid_FB_Token()
            {
                CreateSubjectUnderTest()
                    .Retrieve(ObjectMother.Create<string>(), ObjectMother.Create<string>());
            }

            [TestMethod, UnitTest]
            [ExpectedException(typeof(UserCredentialsException))]
            public void Retrieve_Throws_UserCredentialsException_When_Secret_Is_A_Valid_FB_Token_But_UserIdentifier_Does_Not_Match_FB_User_Id()
            {
                CreateSubjectUnderTest()
                    .Retrieve(ObjectMother.Create<string>(), Token);
            }

            [TestMethod, UnitTest]
            public void Retrieve_Returns_The_Instance_Of_ClaimsIdentity_Returned_Internally_When_Secret_Is_A_Valid_FB_Token_And_UserIdentifier_Matches_FB_User_Id()
            {
                var claimsIdentity = new ClaimsIdentity();

                Assert.AreSame(
                    claimsIdentity,
                    CreateSubjectUnderTest(claimsIdentity).Retrieve(UserId, Token));
            }

            private static ClaimsIdentityRetriever CreateSubjectUnderTest(
                ClaimsIdentity identity =  null)
            {
                return new TestClaimsIdentityRetriever(
                    identity ?? new ClaimsIdentity());
            }

            private static string Token => GetSetting("FacebookMarketingUserInfoClient.Token");

            private static string UserId => GetSetting("FacebookMarketingUserInfoClient.UserId");

            private static string GetSetting(string key) => ConfigurationManager.AppSettings[key];
        }
    }
}
