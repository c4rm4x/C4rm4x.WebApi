#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Validation.Test.Validators
{
    public partial class ExactLengthValidatorTest
    {
        [TestClass]
        public class ExactLengthValidatorValidateTest :
            AbstractValidatorTest<ExactLengthValidator>
        {
            private const int Length = 5;

            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationErrors_When_Value_Is_Null()
            {
                var errors = Validate(null);

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationErrors_When_Value_Length_Is_The_Expected_One()
            {
                var errors = Validate(ObjectMother.Create(Length));

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_A_ValidationError_When_Value_Length_Is_Greather_Than_Expected_One()
            {
                var value = ObjectMother.Create(Length + GetRand(10));
                var errors = Validate(value);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.AreEqual("Error", error.ErrorDescription);
                Assert.AreEqual("TestProperty", error.PropertyName);
                Assert.AreEqual(value, error.PropertyValue);
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_A_ValidationError_When_Value_Length_Is_Less_Than_Expected_One()
            {
                var value = ObjectMother.Create(Length - GetRand(Length));
                var errors = Validate(value);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.AreEqual("Error", error.ErrorDescription);
                Assert.AreEqual("TestProperty", error.PropertyName);
                Assert.AreEqual(value, error.PropertyValue);
            }

            protected override ExactLengthValidator CreateSubjectUnderTest()
            {
                return new ExactLengthValidator(Length, "Error");
            }

            private static int GetRand(int maxValue)
            {
                return new Random().Next(1, maxValue);
            }
        }
    }
}
