#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Monitoring.Core.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Monitoring.Core.Test.Controllers
{
    public partial class MonitorRequestValidatorTest
    {
        [TestClass]
        public class MonitorRequestValidatorValidateTest
        {
            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationError_When_Components_Is_Empty_Collection()
            {
                Assert.IsFalse(
                    CreateSubjectUnderTest()
                        .Validate(new MonitorRequestBuilder()
                            .WithoutComponents()
                            .Build())
                        .Any());
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_A_ValidationError_When_Components_Contains_At_Least_One_Invalid_Component()
            {
                var errors = CreateSubjectUnderTest()
                    .Validate(new MonitorRequestBuilder()
                        .WithComponents(GetComponents(GetInvalidComponent).ToArray())
                        .Build());

                Assert.IsTrue(errors.Any());

                var error = errors.First();

                Assert.IsNotNull(error);
                Assert.AreEqual("Components", error.PropertyName);                
                Assert.AreEqual("Components is not a collection of valid ComponentDto's", error.ErrorDescription);
            }

            [TestMethod, UnitTest]
            public void Validate_Returns_No_ValidationErrors_When_Components_Is_Not_An_Empty_Collection_Of_Valid_Components()
            {
                Assert.IsFalse(
                    CreateSubjectUnderTest()
                        .Validate(new MonitorRequestBuilder()
                            .WithComponents(GetComponents(GetValidComponent).ToArray())
                            .Build())
                        .Any());
            }

            private static MonitorRequestValidator CreateSubjectUnderTest()
            {
                return new MonitorRequestValidator();
            }

            private static IEnumerable<ComponentDto> GetComponents(
                Func<ComponentDto> getComponent)
            {
                var numberOrComponents = GetRand(10);

                for (var i = 0; i < numberOrComponents; i++)
                    yield return getComponent();
            }

            private static int GetRand(int max)
            {
                return new Random().Next(1, max);
            }

            private static ComponentDto GetInvalidComponent()
            {
                return new ComponentDtoBuilder()
                        .WithoutIdentifier()
                        .WithoutName()
                        .Build();
            }

            private static ComponentDto GetValidComponent()
            {
                return new ComponentDtoBuilder()
                    .Build();
            }
        }
    }
}
