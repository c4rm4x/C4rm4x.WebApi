#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Configuration.Controllers;
using C4rm4x.WebApi.Configuration.Test.Controllers.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Configuration.Test.Controllers
{
    public partial class GetConfigurationRequestValidatorTest
    {
        [TestClass]
        public class GetConfigurationRequestValidatorValidateAsyncTest :
            AutoMockFixture<GetConfigurationRequestValidator>
        {
            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_1_ValidationError_When_AppIdentifier_Is_Null()
            {
                var errors = await _sut.ValidateAsync(
                    new GetConfigurationRequestBuilder()
                        .WithoutAppIdentifier()
                        .Build());

                Assert.IsTrue(errors.Any());

                var error = errors.First();
                Assert.IsNotNull(error);
                Assert.AreEqual("AppIdentifier", error.PropertyName);
                Assert.IsNull(error.PropertyValue);
                Assert.AreEqual("Cannot be empty", error.ErrorDescription);
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_1_ValidationError_When_AppIdentifier_Is_Empty_String()
            {
                var errors = await _sut.ValidateAsync(
                    new GetConfigurationRequestBuilder()
                        .WithAppIdentifier(string.Empty)
                        .Build());

                Assert.IsTrue(errors.Any());

                var error = errors.First();
                Assert.IsNotNull(error);
                Assert.AreEqual("AppIdentifier", error.PropertyName);
                Assert.AreEqual(string.Empty, error.PropertyValue);
                Assert.AreEqual("Cannot be empty", error.ErrorDescription);
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_1_ValidationError_When_Version_Is_Null()
            {
                var errors = await _sut.ValidateAsync(
                    new GetConfigurationRequestBuilder()
                        .WithoutVersion()
                        .Build());

                Assert.IsTrue(errors.Any());

                var error = errors.First();
                Assert.IsNotNull(error);
                Assert.AreEqual("Version", error.PropertyName);
                Assert.IsNull(error.PropertyValue);
                Assert.AreEqual("Cannot be empty", error.ErrorDescription);
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_1_ValidationError_When_Version_Is_Empty_String()
            {
                var errors = await _sut.ValidateAsync(
                    new GetConfigurationRequestBuilder()
                        .WithVersion(string.Empty)
                        .Build());

                Assert.IsTrue(errors.Any());

                var error = errors.First();
                Assert.IsNotNull(error);
                Assert.AreEqual("Version", error.PropertyName);
                Assert.AreEqual(string.Empty, error.PropertyValue);
                Assert.AreEqual("Cannot be empty", error.ErrorDescription);
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_0_ValidationError_When_All_Conditions_Are_Met()
            {
                var errors = await _sut.ValidateAsync(
                    new GetConfigurationRequestBuilder().Build());

                Assert.IsFalse(errors.Any());
            }
        }
    }
}
