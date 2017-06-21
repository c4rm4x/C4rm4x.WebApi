#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

#endregion

namespace C4rm4x.WebApi.Validation.Test
{
    public partial class AbstractValidatorFactoryTest
    {
        [TestClass]
        public class AbstractValidatorFactoryGetValidatorTest
        {
            #region Helper classes

            class TestClass
            {
                public TestClass(string testProperty)
                {
                    TestProperty = testProperty;
                }

                public string TestProperty { get; set; }
            }

            class OtherTestClass : TestClass
            {
                public OtherTestClass(string testProperty)
                    : base(testProperty)
                { }
            }

            class TestClassValidator : AbstractValidator<TestClass>
            {
            }

            class TestValidatorFactory : AbstractValidatorFactory
            {
                protected override IValidator CreateInstance(Type type)
                {
                    if (type == typeof(TestClass))
                        return new TestClassValidator();

                    return null;
                }
            }

            #endregion

            [TestMethod, UnitTest]
            public void GetValidator_Returns_An_Instance_Of_IValidator_For_TestClass()
            {
                var validator = GetValidatorFactory().GetValidator<TestClass>();

                Assert.IsNotNull(validator);
                Assert.IsInstanceOfType(validator, typeof(IValidator<TestClass>));
            }

            [TestMethod, UnitTest]
            public void GetValidator_Returns_An_Instance_Of_IValidator()
            {
                var validator = GetValidatorFactory().GetValidator(typeof(TestClass));

                Assert.IsNotNull(validator);
                Assert.IsInstanceOfType(validator, typeof(IValidator));
            }

            [TestMethod, UnitTest]
            public void GetValidator_Returns_An_Instance_Of_IValidator_For_TestClass_When_OtherTetsClass_Is_Requested()
            {
                var validator = GetValidatorFactory().GetValidator(typeof(OtherTestClass));

                Assert.IsNotNull(validator);
                Assert.IsInstanceOfType(validator, typeof(IValidator<TestClass>));
            }

            [TestMethod, UnitTest]
            [ExpectedException(typeof(ArgumentException))]
            public void GetValidator_Throws_Exception_When_Type_To_Validate_Is_Object()
            {
                GetValidatorFactory().GetValidator(typeof(object));
            }

            private static AbstractValidatorFactory GetValidatorFactory()
            {
                return new TestValidatorFactory();
            }
        }
    }
}
