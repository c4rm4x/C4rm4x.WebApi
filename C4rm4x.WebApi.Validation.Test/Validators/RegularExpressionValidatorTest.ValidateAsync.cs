#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Validation.Test.Validators
{
    public partial class RegularExpressionValidatorTest
    {
        [TestClass]
        public class RegularExpressionValidatorValidateAsyncTest :
            AbstractValidatorTest<RegularExpressionValidator>
        {
            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_No_ValidationErrors_When_Value_Is_Null()
            {
                var errors = await ValidateAsync(null);

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_No_ValidationErrors_When_Value_Satisfies_Regular_Expression()
            {
                var errors = await ValidateAsync(ObjectMother.Create<int>().ToString());

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_A_ValidationError_When_Value_Does_Not_Satisfy_Regular_Expression()
            {
                var value = ObjectMother.Create<string>() + "a"; // Enforce string to have, at least, one non numeric character
                var errors = await ValidateAsync(value);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual(value, error.PropertyValue);
                Assert.AreEqual("TestProperty", error.PropertyName);
                Assert.AreEqual("Error", error.ErrorDescription);
            }

            protected override RegularExpressionValidator CreateSubjectUnderTest()
            {
                return new RegularExpressionValidator("[0-9]+", RegexOptions.None, "Error");
            }
        }
    }
}
