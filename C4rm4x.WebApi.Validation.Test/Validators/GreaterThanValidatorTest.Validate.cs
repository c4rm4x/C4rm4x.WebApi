#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Validation.Test.Validators
{
    public partial class GreaterThanValidatorTest
    {
        [TestClass]
        public class GreaterThanValidatorValidateTest :
            AbstractValidatorTest<GreaterThanValidator>
        {
            private const int ValueToCompare = 5;

            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationErrors_When_Value_Is_Greater_Than_Expected()
            {
                var value = ValueToCompare + GetRand();
                var errors = Validate(value);

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationErros_When_Value_Is_Null()
            {
                var errors = Validate(null);

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_A_ValidationError_When_Value_Is_The_Same()
            {
                var errors = Validate(ValueToCompare);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual("Error", error.ErrorDescription);
                Assert.AreEqual("TestProperty", error.PropertyName);
                Assert.AreEqual(ValueToCompare, error.PropertyValue);
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_A_ValidationError_When_Value_Is_Less_Than_Expected()
            {
                var value = ValueToCompare - GetRand();
                var errors = Validate(value);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual("Error", error.ErrorDescription);
                Assert.AreEqual("TestProperty", error.PropertyName);
                Assert.AreEqual(value, error.PropertyValue);
            }

            protected override GreaterThanValidator CreateSubjectUnderTest()
            {
                return new GreaterThanValidator(ValueToCompare, "Error");
            }

            private static int GetRand()
            {
                return new Random().Next(1, 100);
            }
        }
    }
}
