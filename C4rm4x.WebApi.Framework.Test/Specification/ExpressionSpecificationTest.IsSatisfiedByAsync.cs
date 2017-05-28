#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Specification
{
    public partial class ExpressionSpecificationTest
    {
        [TestClass]
        public class ExpressionSpecificationIsSatisfiedByAsyncTest
        {
            [TestMethod, UnitTest]
            public async Task IsSatisfyByAsync_Returns_True_When_Specification_Is_Fulfilled()
            {
                Assert.IsTrue(
                    await GetSubjectUnderTest(true)
                        .IsSatisfiedByAsync(ObjectMother.Create<TestEntity>()));
            }

            [TestMethod, UnitTest]
            public async Task IsSatisfyByAsync_Returns_False_When_Specification_Is_Not_Fulfilled()
            {
                Assert.IsFalse(
                    await GetSubjectUnderTest(false)
                        .IsSatisfiedByAsync(ObjectMother.Create<TestEntity>()));
            }
        }
    }
}
