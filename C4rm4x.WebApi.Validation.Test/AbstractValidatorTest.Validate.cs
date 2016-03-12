#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Validation.Test
{
    public partial class AbstractValidatorTest
    {
        [TestClass]
        public class AbstractValidatorValidateTest
        {
            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationErrors_When_TestProperty_Is_Not_Null_And_RuleSet_Is_Not_Specified()
            {
                Assert.IsFalse(Validate(ObjectMother.Create<string>()).Any());
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_A_ValidationError_When_TestProperty_Is_Null_And_RuleSet_Is_Not_Specified()
            {
                Assert.IsTrue(Validate(null as string).Any());
            }

            [TestMethod, UnitTest]
            [ExpectedException(typeof(ArgumentException))]
            public void Validate_Throws_ArgumentException_When_Object_Is_Not_An_Instance_Of_TestClass_And_RuleSet_Is_Not_Specified()
            {
                Validate(new Object());
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationErrors_When_TestProperty_Is_TestValue_And_RuleSet_Is_Rule()
            {
                Assert.IsFalse(Validate(TestValue, RuleSet_Rule).Any());
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_A_ValidationError_When_TestProperty_Is_Not_TestValue_And_RuleSet_Is_Rule()
            {
                Assert.IsTrue(Validate(ObjectMother.Create<string>(), RuleSet_Rule).Any());
            }

            [TestMethod, UnitTest]
            [ExpectedException(typeof(ArgumentException))]
            public void Validate_Throws_ArgumentException_When_Object_Is_Not_An_Instance_Of_TestClass_And_RuleSet_Is_Rule()
            {
                Validate(new Object(), RuleSet_Rule);
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_A_ValidationError_When_Value_Is_Null_And_No_RuleSet_Is_Specified()
            {
                var errors = Validate(null);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual(TestProperty, error.PropertyName);
                Assert.IsNull(error.PropertyValue);
                Assert.AreEqual("TestProperty cannot be null", error.ErrorDescription);
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationErrors_When_Value_Is_Empty_String_And_No_RuleSet_Is_Specified()
            {
                var errors = Validate(string.Empty);

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationErrors_When_Value_Is_Not_Empty_String_And_No_RuleSet_Is_Specified()
            {
                var errors = Validate(ObjectMother.Create<string>());

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_A_ValidationError_When_Value_Is_Null_And_RuleSet_Is_Rule()
            {
                var errors = Validate(null, RuleSet_Rule);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual(TestProperty, error.PropertyName);
                Assert.IsNull(error.PropertyValue);
                Assert.AreEqual("TestProperty must be equal to TestValue", error.ErrorDescription);
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_A_ValidationError_When_Value_Is_Empty_String_And_RuleSet_Is_Rule()
            {
                var errors = Validate(string.Empty, RuleSet_Rule);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual("TestProperty", error.PropertyName);
                Assert.AreEqual(string.Empty, error.PropertyValue);
                Assert.AreEqual("TestProperty must be equal to TestValue", error.ErrorDescription);
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_A_ValidationError_When_Value_Is_Not_TestValue_And_RuleSet_Is_Rule()
            {
                var value = ObjectMother.Create<string>();
                var errors = Validate(value, RuleSet_Rule);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual(TestProperty, error.PropertyName);
                Assert.AreEqual(value, error.PropertyValue);
                Assert.AreEqual("TestProperty must be equal to TestValue", error.ErrorDescription);
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationErrors_When_Value_Is_TestValue_And_RuleSet_is_Rule()
            {
                var errors = Validate(TestValue, RuleSet_Rule);

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_A_ValidationError_When_Value_Is_Not_Empty_And_Length_Is_Higher_Than_2_And_RuleSet_Is_Conditional()
            {
                var value = ObjectMother.Create(Rand(3, 10));
                var errors = Validate(value, RuleSet_Conditional);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual(TestProperty, error.PropertyName);
                Assert.AreEqual(value, error.PropertyValue);
                Assert.AreEqual("Length must be less than or equal to 2", error.ErrorDescription);
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationErrors_When_Value_Is_Not_Empty_And_Length_Is_Less_Than_Or_Equal_To_2_And_RuleSet_Is_Conditional()
            {
                var errors = Validate(ObjectMother.Create(Rand(1, 2)), RuleSet_Conditional);

                Assert.IsFalse(errors.Any());
            }

            private static TestClassValidator GetValidator()
            {
                return new TestClassValidator();
            }

            private static List<ValidationError> Validate(string testProperty)
            {
                return Validate(new TestClass(testProperty));
            }

            private static List<ValidationError> Validate(object objectToValidate)
            {
                return GetValidator().Validate(objectToValidate);
            }

            private static List<ValidationError> Validate(
                string testProperty,
                string ruleSet)
            {
                return Validate(new TestClass(testProperty), ruleSet);
            }

            private static List<ValidationError> Validate(
                object objectToValidate,
                string ruleSet)
            {
                return GetValidator().Validate(objectToValidate, ruleSet);
            }

            private static int Rand(int min, int max)
            {
                return new Random().Next(min, max);
            }
        }
    }
}
