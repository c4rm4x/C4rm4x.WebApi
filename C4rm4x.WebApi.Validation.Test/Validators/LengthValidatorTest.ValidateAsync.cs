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
    public partial class LengthValidatorTest
    {
        [TestClass]
        public class LengthValidatorValidateAsyncTest :
            AbstractValidatorTest<LengthValidator>
        {
            private const int MinimumLength = 5;
            private const int MaximumLength = 20;

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_No_ValidationErrors_When_Value_Is_Null()
            {
                var errors = await ValidateAsync(null);

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_No_ValidationErrors_When_Value_Lenght_Is_Equal_To_MinimunLength()
            {
                var errors = await ValidateAsync(ObjectMother.Create(MinimumLength));

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_No_ValidationErrors_When_Value_Lenght_Is_Equal_To_MaximunLength()
            {
                var errors = await ValidateAsync(ObjectMother.Create(MaximumLength));

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_No_ValidationErrors_When_Value_Lenght_Is_Between_MinimumLength_And_MaximunLength()
            {
                var errors = await ValidateAsync(ObjectMother.Create(MinimumLength + GetRand(MaximumLength - MinimumLength)));

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_A_ValidationError_When_Value_Lenght_Is_Less_Than_MinimunLength()
            {
                var value = ObjectMother.Create(MinimumLength - GetRand(MinimumLength));
                var errors = await ValidateAsync(value);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual("TestProperty", error.PropertyName);
                Assert.AreEqual(value, error.PropertyValue);
                Assert.AreEqual("Error", error.ErrorDescription);
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_A_ValidationError_When_Value_Lenght_Is_Greater_Than_MinimunLength()
            {
                var value = ObjectMother.Create(MaximumLength + GetRand(MaximumLength));
                var errors = await ValidateAsync(value);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual("TestProperty", error.PropertyName);
                Assert.AreEqual(value, error.PropertyValue);
                Assert.AreEqual("Error", error.ErrorDescription);
            }

            protected override LengthValidator CreateSubjectUnderTest()
            {
                return new LengthValidator(MinimumLength, MaximumLength, "Error");
            }

            private static int GetRand(int maxValue)
            {
                return new Random().Next(1, maxValue);
            }
        }
    }
}
