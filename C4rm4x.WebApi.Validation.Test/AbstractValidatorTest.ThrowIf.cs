#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

#endregion

namespace C4rm4x.WebApi.Validation.Test
{
    public partial class AbstractValidatorTest
    {
        [TestClass]
        public class AbstractValidatorThrowIfTest
        {
            [TestMethod, UnitTest]
            public void ThrowIf_Does_Not_Throw_ValidationException_When_TestProperty_Is_Not_Null_And_RuleSet_Is_Not_Specified()
            {
                ThrowIf(ObjectMother.Create<string>());
            }

            [TestMethod, UnitTest]
            [ExpectedException(typeof(ValidationException))]
            public void ThrowIf_Throws_ValidationException_When_TestProperty_Is_Null_And_RuleSet_Is_Not_Specified()
            {
                ThrowIf(null as string);
            }

            [TestMethod, UnitTest]
            [ExpectedException(typeof(ArgumentException))]
            public void ThrowIf_Throws_ArgumentException_When_Object_Is_Not_An_Instance_Of_TestClass_And_RuleSet_Is_Not_Specified()
            {
                ThrowIf(new Object());
            }

            [TestMethod, UnitTest]
            public void ThrowIf_Does_Not_Throw_ValidationException_When_TestProperty_Is_TestValue_And_RuleSet_Is_Rule()
            {
                ThrowIf("TestValue", "Rule");
            }

            [TestMethod, UnitTest]
            [ExpectedException(typeof(ValidationException))]
            public void ThrowIf_Throws_ValidationException_When_TestProperty_Is_Not_TestValue_And_RuleSet_Is_Rule()
            {
                ThrowIf(ObjectMother.Create<string>(), "Rule");
            }

            [TestMethod, UnitTest]
            [ExpectedException(typeof(ArgumentException))]
            public void ThrowIf_Throws_ArgumentException_When_Object_Is_Not_An_Instance_Of_TestClass_And_RuleSet_Is_Rule()
            {
                ThrowIf(new Object(), "Rule");
            }

            private static void ThrowIf(string testProperty)
            {
                ThrowIf(new TestClass(testProperty));
            }

            private static void ThrowIf(object objectToValidate)
            {
                GetValidator().ThrowIf(objectToValidate);
            }

            private static void ThrowIf(string testProperty, string ruleSet)
            {
                ThrowIf(new TestClass(testProperty), ruleSet);
            }

            private static void ThrowIf(object objectToValidate, string ruleSet)
            {
                GetValidator().ThrowIf(objectToValidate, ruleSet);
            }
        }
    }
}
