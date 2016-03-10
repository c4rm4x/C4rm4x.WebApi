#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Validation.Test.Validators
{
    public partial class CollectionValidatorTest
    {
        [TestClass]
        public class CollectionValidatorValidateTest :
            AbstractValidatorTest<CollectionValidator<TestClass>>
        {
            #region Helper classes

            class TestClassValidator : AbstractValidator<TestClass>
            {
                public TestClassValidator()
                {
                    RuleFor(c => c.TestProperty).NotNull();
                }
            }

            #endregion

            [TestMethod, UnitTest]
            public void Validate_Returns_A_ValidationError_When_Value_Is_Not_A_Collection_Of_T()
            {
                var value = ObjectMother.Create(10);
                var errors = Validate(value);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual("TestProperty", error.PropertyName);
                Assert.AreEqual(value, error.PropertyValue);
                Assert.AreEqual("Error", error.ErrorDescription);
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_A_ValidationError_When_At_Least_One_Of_The_Elements_Of_The_Collection_Is_Invalid()
            {
                var value = GetObjects(() => new TestClass()).ToList();
                var errors = Validate(value);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual("TestProperty", error.PropertyName);
                Assert.AreSame(value, error.PropertyValue);
                Assert.AreEqual("Error", error.ErrorDescription);
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationErrors_When_All_Elements_Of_The_Collection_Are_Valid()
            {
                var errors = Validate(GetObjects(() => new TestClass
                {
                    TestProperty = ObjectMother.Create(GetRand(5))
                })
                .ToList());

                Assert.IsFalse(errors.Any());
            }

            protected override CollectionValidator<TestClass> CreateSubjectUnderTest()
            {
                return new CollectionValidator<TestClass>(
                    () => new TestClassValidator(), "Error");
            }

            private static int GetRand(int max)
            {
                return new Random().Next(1, max);
            }

            private static IEnumerable<TestClass> GetObjects(Func<TestClass> generator)
            {
                var numberOfObjects = GetRand(10);

                for (var i = 0; i < numberOfObjects; i++)
                    yield return generator();
            }
        }
    }
}
