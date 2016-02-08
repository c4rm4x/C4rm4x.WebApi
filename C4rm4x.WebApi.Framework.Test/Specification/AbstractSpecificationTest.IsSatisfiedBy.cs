#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Specification
{

    public partial class AbstractSpecificationTest
    {
        [TestClass]
        public class AbstractSpecificationIsSatisfiedByTest
        {
            [TestMethod, UnitTest]
            public void IsSatisfyBy_Returns_True_When_Specification_Is_Fulfilled()
            {
                Assert.IsTrue(
                    GetSubjectUnderTest(true)
                        .IsSatisfiedBy(ObjectMother.Create<TestEntity>()));
            }

            [TestMethod, UnitTest]
            public void IsSatisfyBy_Returns_False_When_Specification_Is_Not_Fulfilled()
            {
                Assert.IsFalse(
                    GetSubjectUnderTest(false)
                        .IsSatisfiedBy(ObjectMother.Create<TestEntity>()));
            }
        }
    }
}
