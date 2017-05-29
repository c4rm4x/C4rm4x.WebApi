#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Validation.Test.Validators
{
    public partial class EqualValidatorTest
    {
        [TestClass]
        public class EqualValidatorValidateAsyncTest : AbstractValidatorTest<EqualValidator>
        {
            private const string PropertyValue = "PropertyValue";

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_No_ValidationErrors_When_Value_Is_The_Same()
            {
                var errors = await ValidateAsync(PropertyValue);

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_A_ValidationError_When_Value_Is_Null()
            {
                var errors = await ValidateAsync(null);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual(null, error.PropertyValue);
                Assert.AreEqual("TestProperty", error.PropertyName);
                Assert.AreEqual("Error", error.ErrorDescription);
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_A_ValidationError_When_Value_Is_Not_The_Same()
            {
                const string NotTheSame = "NotTheSame";

                var errors = await ValidateAsync(NotTheSame);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual(NotTheSame, error.PropertyValue);
                Assert.AreEqual("TestProperty", error.PropertyName);
                Assert.AreEqual("Error", error.ErrorDescription);
            }

            protected override EqualValidator CreateSubjectUnderTest()
            {
                return new EqualValidator(PropertyValue, "Error");
            }
        }
    }
}
