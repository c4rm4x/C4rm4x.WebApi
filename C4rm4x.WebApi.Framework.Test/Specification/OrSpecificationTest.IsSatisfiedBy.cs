#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Specification
{

    public partial class OrSpecificationTest
    {
        [TestClass]
        public class OrSpecificationIsSatisfiedByTest
        {
            [TestMethod, UnitTest]
            public void IsSatisfiedBy_Returns_True_When_At_Least_LeftSpecification_Is_Fulfilled()
            {
                Assert.IsTrue(
                    GetSubjectUnderTest(true, It.IsAny<bool>())
                        .IsSatisfiedBy(ObjectMother.Create<TestEntity>()));
            }

            [TestMethod, UnitTest]
            public void IsSatisfiedBy_Returns_True_When_At_Least_RightSpecification_Is_Fulfilled()
            {
                Assert.IsTrue(
                    GetSubjectUnderTest(It.IsAny<bool>(), true)
                        .IsSatisfiedBy(ObjectMother.Create<TestEntity>()));
            }

            [TestMethod, UnitTest]
            public void IsSatisfiedBy_Returns_True_When_Both_LeftSpecification_And_RightSpecification_Are_Fulfilled()
            {
                Assert.IsTrue(
                    GetSubjectUnderTest(true, true)
                        .IsSatisfiedBy(ObjectMother.Create<TestEntity>()));
            }

            [TestMethod, UnitTest]
            public void IsSatisfiedBy_Returns_False_When_Both_LeftSpecification_And_RightSpecification_Are_Not_Fulfilled()
            {
                Assert.IsFalse(
                    GetSubjectUnderTest(false, false)
                        .IsSatisfiedBy(ObjectMother.Create<TestEntity>()));
            }
        }
    }
}
