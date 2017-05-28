#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Validation.Test.Validators
{
    public partial class NotNullValidatorTest
    {
        [TestClass]
        public class NotNullValidatorValidateTest :
            AbstractValidatorTest<NotNullValidator>
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
            public void Validate_Returns_No_ValidationErrors_When_Value_Is_Empty_String()
            {
                var errors = Validate(string.Empty);

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationErrors_When_Value_Is_A_Not_Empty_String()
            {
                var errors = Validate(ObjectMother.Create<string>());

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationErrors_When_Value_Is_Empty_Collection()
            {
                var errors = Validate(new List<object>());

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationErrors_When_Value_Is_A_Not_Null_Object()
            {
                var errors = Validate(new object());

                Assert.IsFalse(errors.Any());
            }

            protected override NotNullValidator CreateSubjectUnderTest()
            {
                return new NotNullValidator("Error");
            }
        }
    }
}
