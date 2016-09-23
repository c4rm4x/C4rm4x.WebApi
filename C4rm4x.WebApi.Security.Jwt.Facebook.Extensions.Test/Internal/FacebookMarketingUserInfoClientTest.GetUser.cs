#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

#endregion

namespace C4rm4x.WebApi.Security.Jwt.Facebook.Test
{
    public partial class FacebookMarketingUserInfoClientTest
    {
        [TestClass]
        public class FacebookMarketingUserInfoClientGetUserTest
        {
            [TestMethod, IntegrationTest]
            public void GetUser_Returns_Null_When_No_Data_Is_Retrieved_Because_Token_Is_Invalid()
            {
                Assert.IsNull(FacebookMarketingUserInfoClient
                    .GetUser(ObjectMother.Create<string>(), ObjectMother.Create<string>()));
            }

            [TestMethod, IntegrationTest]
            public void GetUser_Returns_Null_When_Token_Is_Valid_But_UserId_Does_Not_Match_Id_Retrieved()
            {
                Assert.IsNull(FacebookMarketingUserInfoClient.GetUser(ObjectMother.Create<string>(), Token));
            }

            [TestMethod, IntegrationTest]
            public void GetUser_Returns_An_Instance_Of_FBUser_When_Token_Is_Valid_And_UserId_Matches_Id_Retrieved()
            {
                var result = FacebookMarketingUserInfoClient.GetUser(UserId, Token);

                Assert.IsNotNull(result);
                Assert.AreEqual(UserId, result.Id);
            }

            private static string Token => GetSetting("FacebookMarketingUserInfoClient.Token");

            private static string UserId => GetSetting("FacebookMarketingUserInfoClient.UserId");

            private static string GetSetting(string key) => ConfigurationManager.AppSettings[key];
        }
    }
}
