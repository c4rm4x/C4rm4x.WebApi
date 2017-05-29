#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Validation.Test.Validators
{
    public partial class LessThanOrEqualValidatorTest
    {
        [TestClass]
        public class LessThanOrEqualValidatorValidateAsyncTest
            : AbstractValidatorTest<LessThanOrEqualValidator>
        {
            private const int ValueToCompare = 10;

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_No_ValidationErrors_When_Value_Is_The_Same()
            {
                var errors = await ValidateAsync(ValueToCompare);

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_No_ValidationErrors_When_Value_Is_Less_Than_Expected_One()
            {
                var errors = await ValidateAsync(ValueToCompare - GetRand());

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_A_ValidationError_When_Value_Is_Greater_Than_Expected_One()
            {
                var value = ValueToCompare + GetRand();
                var errors = await ValidateAsync(value);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual("TestProperty", error.PropertyName);
                Assert.AreEqual(value, error.PropertyValue);
                Assert.AreEqual("Error", error.ErrorDescription);
            }

            protected override LessThanOrEqualValidator CreateSubjectUnderTest()
            {
                return new LessThanOrEqualValidator(ValueToCompare, "Error");
            }

            private static int GetRand()
            {
                return new Random().Next(1, 100);
            }
        }
    }
}
