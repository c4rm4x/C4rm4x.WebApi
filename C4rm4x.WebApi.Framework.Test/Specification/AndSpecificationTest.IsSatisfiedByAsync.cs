#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Specification
{

    public partial class AndSpecificationTest
    {
        [TestClass]
        public class AndSpecificationIsSatisfiedByAsyncTest
        {
            [TestMethod, UnitTest]
            public async Task IsSatisfiedByAsync_Returns_False_When_Only_LeftSpecification_Is_Fulfilled()
            {
                Assert.IsFalse(
                    await GetSubjectUnderTest(true, false)
                        .IsSatisfiedByAsync(ObjectMother.Create<TestEntity>(), new List<ValidationError>()));
            }

            [TestMethod, UnitTest]
            public async Task IsSatisfiedByAsync_Returns_False_When_Only_RightSpecification_Is_Fulfilled()
            {
                Assert.IsFalse(
                   await GetSubjectUnderTest(false, true)
                        .IsSatisfiedByAsync(ObjectMother.Create<TestEntity>(), new List<ValidationError>()));
            }

            [TestMethod, UnitTest]
            public async Task IsSatisfiedByAsync_Returns_False_When_Both_LeftSpecification_And_RightSpecification_Are_Not_Fulfilled()
            {
                Assert.IsFalse(
                    await GetSubjectUnderTest(false, false)
                        .IsSatisfiedByAsync(ObjectMother.Create<TestEntity>(), new List<ValidationError>()));
            }

            [TestMethod, UnitTest]
            public async Task IsSatisfiedByAsync_Returns_True_When_Both_LeftSpecification_And_RightSpecification_Are_Fulfilled()
            {
                Assert.IsTrue(
                    await GetSubjectUnderTest(true, true)
                        .IsSatisfiedByAsync(ObjectMother.Create<TestEntity>(), new List<ValidationError>()));
            }
        }
    }
}
