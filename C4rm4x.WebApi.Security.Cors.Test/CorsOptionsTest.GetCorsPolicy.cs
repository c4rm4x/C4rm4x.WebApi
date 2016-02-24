#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Web.Cors;

#endregion

namespace C4rm4x.WebApi.Security.Cors.Test
{
    public partial class CorsOptionsTest
    {
        [TestClass]
        public class CorsOptionsGetCorsPolicyTest
        {
            [TestMethod, UnitTest]
            public void GetCorsPolicy_Returns_An_Instance_Of_CorsPolicy_Whith_AllowAnyHeader_As_True_When_CorsOptions_AllowAnyHeader_Is_True()
            {
                Assert.IsTrue(
                    GetCorsPolicy(allowedHeadersSeparatedByComma: CorsOptions.AnyHeader)
                        .AllowAnyHeader);
            }

            [TestMethod, UnitTest]
            public void GetCorsPolicy_Returns_An_Instance_Of_CorsPolicy_Whith_Headers_As_Empty_List_When_CorsOptions_AllowAnyHeader_Is_True()
            {
                Assert.IsFalse(
                    GetCorsPolicy(allowedHeadersSeparatedByComma: CorsOptions.AnyHeader)
                        .Headers
                        .Any());
            }

            [TestMethod, UnitTest]
            public void GetCorsPolicy_Returns_An_Instance_Of_CorsPolicy_Whith_AllowAnyHeader_As_False_When_CorsOptions_AllowAnyHeader_Is_False()
            {
                Assert.IsFalse(
                    GetCorsPolicy(allowedHeadersSeparatedByComma: ObjectMother.Create<string>())
                        .AllowAnyHeader);
            }

            [TestMethod, UnitTest]
            public void GetCorsPolicy_Returns_An_Instance_Of_CorsPolicy_Whith_Headers_As_Not_Empty_List_When_CorsOptions_AllowAnyHeader_Is_True()
            {
                Assert.IsTrue(
                    GetCorsPolicy(allowedHeadersSeparatedByComma: ObjectMother.Create<string>())
                        .Headers
                        .Any());
            }

            [TestMethod, UnitTest]
            public void GetCorsPolicy_Returns_An_Instance_Of_CorsPolicy_Whith_AllowAnyMethod_As_True_When_CorsOptions_AllowAnyMethod_Is_True()
            {
                Assert.IsTrue(
                    GetCorsPolicy(allowedMethodsSeparatedByComma: CorsOptions.AnyMethod)
                        .AllowAnyMethod);
            }

            [TestMethod, UnitTest]
            public void GetCorsPolicy_Returns_An_Instance_Of_CorsPolicy_Whith_Methods_As_Empty_List_When_CorsOptions_AllowAnyMethod_Is_True()
            {
                Assert.IsFalse(
                    GetCorsPolicy(allowedMethodsSeparatedByComma: CorsOptions.AnyMethod)
                        .Methods
                        .Any());
            }

            [TestMethod, UnitTest]
            public void GetCorsPolicy_Returns_An_Instance_Of_CorsPolicy_Whith_AllowAnyMethod_As_False_When_CorsOptions_AllowAnyMethod_Is_False()
            {
                Assert.IsFalse(
                    GetCorsPolicy(allowedMethodsSeparatedByComma: ObjectMother.Create<string>())
                        .AllowAnyMethod);
            }

            [TestMethod, UnitTest]
            public void GetCorsPolicy_Returns_An_Instance_Of_CorsPolicy_Whith_Methods_As_Not_Empty_List_When_CorsOptions_AllowAnyMethod_Is_True()
            {
                Assert.IsTrue(
                    GetCorsPolicy(allowedMethodsSeparatedByComma: ObjectMother.Create<string>())
                        .Methods
                        .Any());
            }

            [TestMethod, UnitTest]
            public void GetCorsPolicy_Returns_An_Instance_Of_CorsPolicy_Whith_AllowAnyOrigin_As_True_When_CorsOptions_AllowAnyOrigin_Is_True()
            {
                Assert.IsTrue(
                    GetCorsPolicy(allowedOriginsSeparatedByComma: CorsOptions.AnyOrigin)
                        .AllowAnyOrigin);
            }

            [TestMethod, UnitTest]
            public void GetCorsPolicy_Returns_An_Instance_Of_CorsPolicy_Whith_Origins_As_Empty_List_When_CorsOptions_AllowAnyOrigin_Is_True()
            {
                Assert.IsFalse(
                    GetCorsPolicy(allowedOriginsSeparatedByComma: CorsOptions.AnyOrigin)
                        .Origins
                        .Any());
            }

            [TestMethod, UnitTest]
            public void GetCorsPolicy_Returns_An_Instance_Of_CorsPolicy_Whith_AllowAnyOrigin_As_False_When_CorsOptions_AllowAnyOrigin_Is_False()
            {
                Assert.IsFalse(
                    GetCorsPolicy(allowedOriginsSeparatedByComma: ObjectMother.Create<string>())
                        .AllowAnyOrigin);
            }

            [TestMethod, UnitTest]
            public void GetCorsPolicy_Returns_An_Instance_Of_CorsPolicy_Whith_Origins_As_Not_Empty_List_When_CorsOptions_AllowAnyOrigin_Is_True()
            {
                Assert.IsTrue(
                    GetCorsPolicy(allowedOriginsSeparatedByComma: ObjectMother.Create<string>())
                        .Origins
                        .Any());
            }

            private static CorsPolicy GetCorsPolicy(
                string allowedOriginsSeparatedByComma = CorsOptions.AnyOrigin,
                string allowedHeadersSeparatedByComma = CorsOptions.AnyHeader,
                string allowedMethodsSeparatedByComma = CorsOptions.AnyMethod)
            {
                return new CorsOptions(
                    allowedOriginsSeparatedByComma,
                    allowedMethodsSeparatedByComma,
                    allowedHeadersSeparatedByComma)
                    .GetCorsPolicy();
            }
        }
    }
}
