#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.Tools.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;

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

            protected override async Task<ClaimsIdentity> RetrieveAsync(FBUser user)
            {
                return await Task.FromResult(ClaimsIdentity);
            }
        }

        #endregion

        [TestClass]
        public class ClaimsIdentityRetrieverRetrieveAsyncTest
        {
            [TestMethod, UnitTest]
            public async Task RetrieveAsync_Returns_Null_When_Secret_Is_Not_A_Valid_FB_Token()
            {
                Assert.IsNull(await CreateSubjectUnderTest()
                    .RetrieveAsync(ObjectMother.Create<string>(), ObjectMother.Create<string>()));
            }

            [TestMethod, UnitTest]
            public async Task RetrieveAsync_Returns_The_Instance_Of_ClaimsIdentity_Returned_Internally_When_Secret_Is_A_Valid_FB_Token_And_UserIdentifier_Matches_FB_User_Id()
            {
                var claimsIdentity = new ClaimsIdentity();

                Assert.AreSame(
                    claimsIdentity,
                    await CreateSubjectUnderTest(claimsIdentity).RetrieveAsync(UserId, Token));
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
