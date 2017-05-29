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
    public partial class MaximumLengthValidatorTest
    {
        [TestClass]
        public class MaximumLengthValidatorValidateAsyncTest :
            AbstractValidatorTest<MaximumLengthValidator>
        {
            private const int MaximumLength = 25;

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_No_ValidationErrors_When_Value_Is_Null()
            {
                var errors = await ValidateAsync(null);

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_No_ValidationErrors_When_Value_Length_Is_Zero()
            {
                var errors = await ValidateAsync(string.Empty);

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_No_ValidationErrors_When_Value_Length_Is_The_Same_Than_Expected_One()
            {
                var errors = await ValidateAsync(ObjectMother.Create(MaximumLength));

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_No_ValidationErrors_When_Value_Length_Is_Less_Than_Expected_One()
            {
                var errors = await ValidateAsync(ObjectMother.Create(GetRand(MaximumLength)));

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_A_ValidationError_When_Value_Length_Is_Greater_Than_Expected_One()
            {
                var value = ObjectMother.Create(MaximumLength + GetRand());
                var errors = await ValidateAsync(value);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual("Error", error.ErrorDescription);
                Assert.AreEqual(value, error.PropertyValue);
                Assert.AreEqual("TestProperty", error.PropertyName);
            }

            protected override MaximumLengthValidator CreateSubjectUnderTest()
            {
                return new MaximumLengthValidator(MaximumLength, "Error");
            }

            private static int GetRand(int maxValue = MaximumLength)
            {
                return new Random().Next(1, maxValue);
            }
        }
    }
}
