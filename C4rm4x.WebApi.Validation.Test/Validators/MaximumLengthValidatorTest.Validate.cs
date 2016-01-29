#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Validation.Test.Validators
{
    public partial class MaximumLengthValidatorTest
    {
        [TestClass]
        public class MaximumLengthValidatorValidateTest :
            AbstractValidatorTest<MaximumLengthValidator>
        {
            private const int MaximumLength = 25;

            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationErrors_When_Value_Is_Null()
            {
                var errors = Validate(null);

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationErrors_When_Value_Length_Is_Zero()
            {
                var errors = Validate(string.Empty);

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationErrors_When_Value_Length_Is_The_Same_Than_Expected_One()
            {
                var errors = Validate(ObjectMother.Create(MaximumLength));

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationErrors_When_Value_Length_Is_Less_Than_Expected_One()
            {
                var errors = Validate(ObjectMother.Create(GetRand(MaximumLength)));

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_A_ValidationError_When_Value_Length_Is_Greater_Than_Expected_One()
            {
                var value = ObjectMother.Create(MaximumLength + GetRand());
                var errors = Validate(value);

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
