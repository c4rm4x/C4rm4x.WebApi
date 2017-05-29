#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Validation.Test.Validators
{
    public partial class NotNullValidatorTest
    {
        [TestClass]
        public class NotNullValidatorValidateAsyncTest :
            AbstractValidatorTest<NotNullValidator>
        {
            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_A_ValidationError_When_Value_Is_Null()
            {
                var errors = await ValidateAsync(null);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.IsNull(error.PropertyValue);
                Assert.AreEqual("Error", error.ErrorDescription);
                Assert.AreEqual("TestProperty", error.PropertyName);
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_No_ValidationErrors_When_Value_Is_Empty_String()
            {
                var errors = await ValidateAsync(string.Empty);

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_No_ValidationErrors_When_Value_Is_A_Not_Empty_String()
            {
                var errors = await ValidateAsync(ObjectMother.Create<string>());

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_No_ValidationErrors_When_Value_Is_Empty_Collection()
            {
                var errors = await ValidateAsync(new List<object>());

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_No_ValidationErrors_When_Value_Is_A_Not_Null_Object()
            {
                var errors = await ValidateAsync(new object());

                Assert.IsFalse(errors.Any());
            }

            protected override NotNullValidator CreateSubjectUnderTest()
            {
                return new NotNullValidator("Error");
            }
        }
    }
}
