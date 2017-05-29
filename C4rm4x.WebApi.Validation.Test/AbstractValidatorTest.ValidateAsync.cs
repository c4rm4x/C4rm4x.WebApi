#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Validation.Test
{
    public partial class AbstractValidatorTest
    {
        [TestClass]
        public class AbstractValidatorValidateTest
        {
            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_No_ValidationErrors_When_TestProperty_Is_Not_Null_And_RuleSet_Is_Not_Specified()
            {
                var results = await ValidateAsync(ObjectMother.Create<string>());
                Assert.IsFalse(results.Any());
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_A_ValidationError_When_TestProperty_Is_Null_And_RuleSet_Is_Not_Specified()
            {
                var results = await ValidateAsync(null as string);
                Assert.IsTrue(results.Any());
            }

            [TestMethod, UnitTest]
            [ExpectedException(typeof(ArgumentException))]
            public async Task ValidateAsync_Throws_ArgumentException_When_Object_Is_Not_An_Instance_Of_TestClass_And_RuleSet_Is_Not_Specified()
            {
                await ValidateAsync(new Object());
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_No_ValidationErrors_When_TestProperty_Is_TestValue_And_RuleSet_Is_Rule()
            {
                var results = await ValidateAsync(TestValue, RuleSet_Rule);
                Assert.IsFalse(results.Any());
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_A_ValidationError_When_TestProperty_Is_Not_TestValue_And_RuleSet_Is_Rule()
            {
                var results = await ValidateAsync(ObjectMother.Create<string>(), RuleSet_Rule);
                Assert.IsTrue(results.Any());
            }

            [TestMethod, UnitTest]
            [ExpectedException(typeof(ArgumentException))]
            public async Task ValidateAsync_Throws_ArgumentException_When_Object_Is_Not_An_Instance_Of_TestClass_And_RuleSet_Is_Rule()
            {
                await ValidateAsync(new Object(), RuleSet_Rule);
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_A_ValidationError_When_Value_Is_Null_And_No_RuleSet_Is_Specified()
            {
                var errors = await ValidateAsync(null);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual(TestProperty, error.PropertyName);
                Assert.IsNull(error.PropertyValue);
                Assert.AreEqual("TestProperty cannot be null", error.ErrorDescription);
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_No_ValidationErrors_When_Value_Is_Empty_String_And_No_RuleSet_Is_Specified()
            {
                var errors = await ValidateAsync(string.Empty);

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_No_ValidationErrors_When_Value_Is_Not_Empty_String_And_No_RuleSet_Is_Specified()
            {
                var errors = await ValidateAsync(ObjectMother.Create<string>());

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_A_ValidationError_When_Value_Is_Null_And_RuleSet_Is_Rule()
            {
                var errors = await ValidateAsync(null, RuleSet_Rule);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual(TestProperty, error.PropertyName);
                Assert.IsNull(error.PropertyValue);
                Assert.AreEqual("TestProperty must be equal to TestValue", error.ErrorDescription);
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_A_ValidationError_When_Value_Is_Empty_String_And_RuleSet_Is_Rule()
            {
                var errors = await ValidateAsync(string.Empty, RuleSet_Rule);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual("TestProperty", error.PropertyName);
                Assert.AreEqual(string.Empty, error.PropertyValue);
                Assert.AreEqual("TestProperty must be equal to TestValue", error.ErrorDescription);
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_A_ValidationError_When_Value_Is_Not_TestValue_And_RuleSet_Is_Rule()
            {
                var value = ObjectMother.Create<string>();
                var errors = await ValidateAsync(value, RuleSet_Rule);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual(TestProperty, error.PropertyName);
                Assert.AreEqual(value, error.PropertyValue);
                Assert.AreEqual("TestProperty must be equal to TestValue", error.ErrorDescription);
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_No_ValidationErrors_When_Value_Is_TestValue_And_RuleSet_is_Rule()
            {
                var errors = await ValidateAsync(TestValue, RuleSet_Rule);

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_A_ValidationError_When_Value_Is_Not_Empty_And_Length_Is_Higher_Than_2_And_RuleSet_Is_Conditional()
            {
                var value = ObjectMother.Create(Rand(3, 10));
                var errors = await ValidateAsync(value, RuleSet_Conditional);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual(TestProperty, error.PropertyName);
                Assert.AreEqual(value, error.PropertyValue);
                Assert.AreEqual("Length must be less than or equal to 2", error.ErrorDescription);
            }

            [TestMethod, UnitTest]
            public async Task ValidateAsync_Returns_No_ValidationErrors_When_Value_Is_Not_Empty_And_Length_Is_Less_Than_Or_Equal_To_2_And_RuleSet_Is_Conditional()
            {
                var errors = await ValidateAsync(ObjectMother.Create(Rand(1, 2)), RuleSet_Conditional);

                Assert.IsFalse(errors.Any());
            }

            private static TestClassValidator GetValidator()
            {
                return new TestClassValidator();
            }

            private static async Task<List<ValidationError>> ValidateAsync(string testProperty)
            {
                return await ValidateAsync(new TestClass(testProperty));
            }

            private static async Task<List<ValidationError>> ValidateAsync(object objectToValidate)
            {
                return await GetValidator().ValidateAsync(objectToValidate);
            }

            private static async Task<List<ValidationError>> ValidateAsync(
                string testProperty,
                string ruleSet)
            {
                return await ValidateAsync(new TestClass(testProperty), ruleSet);
            }

            private static async Task<List<ValidationError>> ValidateAsync(
                object objectToValidate,
                string ruleSet)
            {
                return await GetValidator().ValidateAsync(objectToValidate, ruleSet);
            }

            private static int Rand(int min, int max)
            {
                return new Random().Next(min, max);
            }
        }
    }
}
