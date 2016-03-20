#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Security.Jwt.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Security.Jwt.Test.Controllers
{
    public partial class GenerateTokenRequestValidatorTest
    {
        [TestClass]
        public class GenerateTokenRequestValidatorValidateTest
            : AutoMockFixture<GenerateTokenRequestValidator>
        {
            [TestMethod, UnitTest]
            public void Validate_Returns_1_ValidationError_When_UserIdentifier_Is_Null()
            {
                var errors = _sut.Validate(
                    new GenerateTokenRequestBuilder()
                        .WithoutUserIdentifier()
                        .Build());

                Assert.IsTrue(errors.Any());

                var error = errors.First();
                Assert.IsNotNull(error);
                Assert.AreEqual("UserIdentifier", error.PropertyName);
                Assert.IsNull(error.PropertyValue);
                Assert.AreEqual("Cannot be empty", error.ErrorDescription);
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_1_ValidationError_When_UserIdentifier_Is_Empty_String()
            {
                var errors = _sut.Validate(
                    new GenerateTokenRequestBuilder()
                        .WithUserIdentifier(string.Empty)
                        .Build());

                Assert.IsTrue(errors.Any());

                var error = errors.First();
                Assert.IsNotNull(error);
                Assert.AreEqual("UserIdentifier", error.PropertyName);
                Assert.AreEqual(string.Empty, error.PropertyValue);
                Assert.AreEqual("Cannot be empty", error.ErrorDescription);
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationErrors_When_UserIdentifier_Is_Neither_Null_Nor_Empty_String()
            {
                Assert.IsFalse(
                    _sut.Validate(
                        new GenerateTokenRequestBuilder().Build())
                    .Any());
            }
        }
    }
}
