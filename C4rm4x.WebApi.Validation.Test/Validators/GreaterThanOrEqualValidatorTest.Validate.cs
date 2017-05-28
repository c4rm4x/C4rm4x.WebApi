﻿#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Validation.Test.Validators
{
    public partial class GreaterThanOrEqualValidatorTest
    {
        [TestClass]
        public class GreaterThanOrEqualValidatorValidateTest :
            AbstractValidatorTest<GreaterThanOrEqualValidator>
        {
            private const int ValueToCompare = 10;

            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationErrors_When_Value_Is_The_Same()
            {
                var errors = Validate(ValueToCompare);

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationErrors_When_Value_Is_Greater_Than_Expected_One()
            {
                var errors = Validate(ValueToCompare + GetRand());

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_A_ValidationError_When_Value_Is_Less_Than_Expected_One()
            {
                var value = ValueToCompare - GetRand();
                var errors = Validate(value);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual("TestProperty", error.PropertyName);
                Assert.AreEqual(value, error.PropertyValue);
                Assert.AreEqual("Error", error.ErrorDescription);
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationErrors_When_Value_Is_Null()
            {
                var errors = Validate(null);

                Assert.IsFalse(errors.Any());
            }

            protected override GreaterThanOrEqualValidator CreateSubjectUnderTest()
            {
                return new GreaterThanOrEqualValidator(ValueToCompare, "Error");
            }

            private static int GetRand()
            {
                return new Random().Next(1, 100);
            }
        }
    }
}
