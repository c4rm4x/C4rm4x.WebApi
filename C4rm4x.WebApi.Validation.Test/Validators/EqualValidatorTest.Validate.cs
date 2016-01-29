#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Validation.Test.Validators
{
    public partial class EqualValidatorTest
    {
        [TestClass]
        public class EqualValidatorValidateTest : AbstractValidatorTest<EqualValidator>
        {
            private const string PropertyValue = "PropertyValue";

            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationErrors_When_Value_Is_The_Same()
            {
                var errors = Validate(PropertyValue);

                Assert.IsFalse(errors.Any());
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_A_ValidationError_When_Value_Is_Null()
            {
                var errors = Validate(null);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual(null, error.PropertyValue);
                Assert.AreEqual("TestProperty", error.PropertyName);
                Assert.AreEqual("Error", error.ErrorDescription);
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_A_ValidationError_When_Value_Is_Not_The_Same()
            {
                const string NotTheSame = "NotTheSame";

                var errors = Validate(NotTheSame);

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual(NotTheSame, error.PropertyValue);
                Assert.AreEqual("TestProperty", error.PropertyName);
                Assert.AreEqual("Error", error.ErrorDescription);
            }

            protected override EqualValidator CreateSubjectUnderTest()
            {
                return new EqualValidator(PropertyValue, "Error");
            }
        }
    }
}
