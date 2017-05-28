#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Specification
{

    public partial class OrSpecificationTest
    {
        [TestClass]
        public class OrSpecificationIsSatisfiedByAsyncTest
        {
            [TestMethod, UnitTest]
            public async Task IsSatisfiedByAsync_Returns_True_When_At_Least_LeftSpecification_Is_Fulfilled()
            {
                Assert.IsTrue(
                    await GetSubjectUnderTest(true, It.IsAny<bool>())
                        .IsSatisfiedByAsync(ObjectMother.Create<TestEntity>()));
            }

            [TestMethod, UnitTest]
            public async Task IsSatisfiedByAsync_Returns_True_When_At_Least_RightSpecification_Is_Fulfilled()
            {
                Assert.IsTrue(
                    await GetSubjectUnderTest(It.IsAny<bool>(), true)
                        .IsSatisfiedByAsync(ObjectMother.Create<TestEntity>()));
            }

            [TestMethod, UnitTest]
            public async Task IsSatisfiedByAsync_Returns_True_When_Both_LeftSpecification_And_RightSpecification_Are_Fulfilled()
            {
                Assert.IsTrue(
                    await GetSubjectUnderTest(true, true)
                        .IsSatisfiedByAsync(ObjectMother.Create<TestEntity>()));
            }

            [TestMethod, UnitTest]
            public async Task IsSatisfiedByAsync_Returns_False_When_Both_LeftSpecification_And_RightSpecification_Are_Not_Fulfilled()
            {
                Assert.IsFalse(
                    await GetSubjectUnderTest(false, false)
                        .IsSatisfiedByAsync(ObjectMother.Create<TestEntity>()));
            }
        }
    }
}
