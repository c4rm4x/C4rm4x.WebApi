#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Specification
{

    public partial class AndSpecificationTest
    {
        [TestClass]
        public class AndSpecificationIsSatisfiedByTest
        {
            [TestMethod, UnitTest]
            public void IsSatisfiedBy_Returns_False_When_Only_LeftSpecification_Is_Fulfilled()
            {
                Assert.IsFalse(
                    GetSubjectUnderTest(true, false)
                        .IsSatisfiedBy(ObjectMother.Create<TestEntity>()));
            }

            [TestMethod, UnitTest]
            public void IsSatisfiedBy_Returns_False_When_Only_RightSpecification_Is_Fulfilled()
            {
                Assert.IsFalse(
                    GetSubjectUnderTest(false, true)
                        .IsSatisfiedBy(ObjectMother.Create<TestEntity>()));
            }

            [TestMethod, UnitTest]
            public void IsSatisfiedBy_Returns_False_When_Both_LeftSpecification_And_RightSpecification_Are_Not_Fulfilled()
            {
                Assert.IsFalse(
                    GetSubjectUnderTest(false, false)
                        .IsSatisfiedBy(ObjectMother.Create<TestEntity>()));
            }

            [TestMethod, UnitTest]
            public void IsSatisfiedBy_Returns_True_When_Both_LeftSpecification_And_RightSpecification_Are_Fulfilled()
            {
                Assert.IsTrue(
                    GetSubjectUnderTest(true, true)
                        .IsSatisfiedBy(ObjectMother.Create<TestEntity>()));
            }
        }
    }
}
