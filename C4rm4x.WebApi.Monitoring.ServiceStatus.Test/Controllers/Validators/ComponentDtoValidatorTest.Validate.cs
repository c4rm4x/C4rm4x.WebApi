#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Test.Controllers
{
    public partial class ComponentDtoValidatorTest
    {
        [TestClass]
        public class ComponentDtoValidatorValidateTest
        {
            [TestMethod, UnitTest]
            public void Validate_Returns_A_ValidationError_When_Identifier_Is_Null()
            {
                var errors = CreateSubjectUnderTest()
                    .Validate(new ComponentDtoBuilder()
                        .WithoutIdentifier()
                        .Build());

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual("Identifier", error.PropertyName);
                Assert.IsNull(error.PropertyValue);
                Assert.AreEqual("Cannot be null", error.ErrorDescription);
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_A_ValidationError_When_Name_Is_Null()
            {
                var errors = CreateSubjectUnderTest()
                    .Validate(new ComponentDtoBuilder()
                        .WithoutName()
                        .Build());

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual("Name", error.PropertyName);
                Assert.IsNull(error.PropertyValue);
                Assert.AreEqual("Cannot be empty", error.ErrorDescription);
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_A_ValidationError_When_Name_Is_Empty_String()
            {
                var errors = CreateSubjectUnderTest()
                    .Validate(new ComponentDtoBuilder()
                        .WithName(string.Empty)
                        .Build());

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual("Name", error.PropertyName);
                Assert.AreEqual(string.Empty, error.PropertyValue);
                Assert.AreEqual("Cannot be empty", error.ErrorDescription);
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_2_ValidationErrors_When_Identifier_Is_Null_And_Name_Is_Null()
            {
                var errors = CreateSubjectUnderTest()
                    .Validate(new ComponentDtoBuilder()
                        .WithoutIdentifier()
                        .WithoutName()
                        .Build());

                Assert.AreEqual(2, errors.Count());
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_2_ValidationErrors_When_Identifier_Is_Null_And_Name_Is_Empty_String()
            {
                var errors = CreateSubjectUnderTest()
                    .Validate(new ComponentDtoBuilder()
                        .WithoutIdentifier()
                        .WithName(string.Empty)
                        .Build());

                Assert.AreEqual(2, errors.Count());
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationErrors_When_Identifier_Is_Not_Null_And_Name_Is_Neither_Null_Nor_Empty_String()
            {
                Assert.IsFalse(
                    CreateSubjectUnderTest()
                        .Validate(new ComponentDtoBuilder().Build()).Any());
            }

            private static ComponentDtoValidator CreateSubjectUnderTest()
            {
                return new ComponentDtoValidator();
            }
        }
    }
}
