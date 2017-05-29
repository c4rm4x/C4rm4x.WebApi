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
    public partial class MinimumLengthValidatorTest
    {
        [TestClass]
        public class MinimumLengthValidatorValidateAsyncTest :
            AbstractValidatorTest<MinimumLengthValidator>
        {
            private const int MinimumLength = 5;

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_No_ValidationErrors_When_Value_Is_Null()
            {
                var errors = await ValidateAsync(null);

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_No_ValidationErrors_When_Value_Length_Is_The_Same_Than_Expected_One()
            {
                var errors = await ValidateAsync(ObjectMother.Create(MinimumLength));

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_No_ValidationErrors_When_Value_Length_Is_Greater_Than_Expected_One()
            {
                var errors = await ValidateAsync(ObjectMother.Create(MinimumLength + GetRand(MinimumLength)));

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_A_ValidationError_When_Value_Length_Is_Less_Than_Expected_One()
            {
                var value = ObjectMother.Create(GetRand(1, MinimumLength));
                var errors = await ValidateAsync(value);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual("Error", error.ErrorDescription);
                Assert.AreEqual(value, error.PropertyValue);
                Assert.AreEqual("TestProperty", error.PropertyName);
            }

            protected override MinimumLengthValidator CreateSubjectUnderTest()
            {
                return new MinimumLengthValidator(MinimumLength, "Error");
            }

            private static int GetRand(int minValue = MinimumLength, int maxValue = 50)
            {
                return new Random().Next(minValue, maxValue);
            }
        }
    }
}
