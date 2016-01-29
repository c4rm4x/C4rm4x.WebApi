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
    public partial class NotEmptyValidatorTest
    {
        [TestClass]
        public class NotEmptyValidatorValidateTest :
            AbstractValidatorTest<NotEmptyValidator>
        {
            [TestMethod, UnitTest]
            public void Validate_Returns_A_ValidationError_When_Value_Is_Null()
            {
                var errors = Validate(null);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.IsNull(error.PropertyValue);
                Assert.AreEqual("Error", error.ErrorDescription);
                Assert.AreEqual("TestProperty", error.PropertyName);
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_A_ValidationError_When_Value_Is_Empty_String()
            {
                var errors = Validate(string.Empty);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual(string.Empty, error.PropertyValue);
                Assert.AreEqual("Error", error.ErrorDescription);
                Assert.AreEqual("TestProperty", error.PropertyName);
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_A_ValidationError_When_Value_Is_An_Empty_Enumerable()
            {
                var emptyEnumerable = GetEnumerable(0).ToList();
                var errors = Validate(emptyEnumerable);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreSame(emptyEnumerable, error.PropertyValue);
                Assert.AreEqual("Error", error.ErrorDescription);
                Assert.AreEqual("TestProperty", error.PropertyName);
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationErrors_When_Value_Is_A_Not_Empty_String()
            {
                var errors = Validate(ObjectMother.Create<string>());

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationErrors_When_Value_Is_Not_An_Empty_Collection()
            {
                var notEmptyEnumerable = GetEnumerable(GetRand()).ToList();
                var errors = Validate(notEmptyEnumerable);

                Assert.IsFalse(errors.Any());
            }

            protected override NotEmptyValidator CreateSubjectUnderTest()
            {
                return new NotEmptyValidator("Error");
            }

            private static int GetRand()
            {
                return new Random().Next(1, 10);
            }

            private static IEnumerable<object> GetEnumerable(int numElements)
            {
                for (int i = 0; i < numElements; i++)
                    yield return new object();
            }
        }
    }
}
