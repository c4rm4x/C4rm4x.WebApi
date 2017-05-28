#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.Specification;
using C4rm4x.WebApi.Framework.Test.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Specification
{
    public partial class NotNullSpecificationTest
    {
        #region Helper classes

        public class TestEntity { }

        class TestNotNullSpecification : NotNullSpecification<TestEntity>
        {
            public TestNotNullSpecification()
                : base(new RuleBuilder().Build())
            {
            }
        }

        #endregion

        [TestClass]
        public class NotNullSpecificationIsSatisfiedByAsyncTest
        {
            [TestMethod, UnitTest]
            public async Task IsSatisfyByAsync_Returns_True_When_Entity_Is_Not_Null()
            {
                Assert.IsTrue(
                    await GetSubjectUnderTest()
                        .IsSatisfiedByAsync(ObjectMother.Create<TestEntity>()));
            }

            [TestMethod, UnitTest]
            public async Task IsSatisfyByAsync_Returns_False_When_Entity_Is_Null()
            {
                Assert.IsFalse(
                    await GetSubjectUnderTest()
                        .IsSatisfiedByAsync(null));
            }

            private static ISpecification<TestEntity> GetSubjectUnderTest()
            {
                return new TestNotNullSpecification();
            }
        }
    }
}
