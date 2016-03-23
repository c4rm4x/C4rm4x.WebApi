#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.Specification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
                : base((entity) => new Exception())
            {
            }
        }

        #endregion

        [TestClass]
        public class NotNullSpecificationIsSatisfiedByTest
        {
            [TestMethod, UnitTest]
            public void IsSatisfyBy_Returns_True_When_Entity_Is_Not_Null()
            {
                Assert.IsTrue(
                    GetSubjectUnderTest()
                        .IsSatisfiedBy(ObjectMother.Create<TestEntity>()));
            }

            [TestMethod, UnitTest]
            public void IsSatisfyBy_Returns_False_When_Entity_Is_Null()
            {
                Assert.IsFalse(
                    GetSubjectUnderTest()
                        .IsSatisfiedBy(null));
            }

            private static ISpecification<TestEntity> GetSubjectUnderTest()
            {
                return new TestNotNullSpecification();
            }
        }
    }
}
