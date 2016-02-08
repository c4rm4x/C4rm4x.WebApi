#region Using

using C4rm4x.WebApi.Framework.Specification;
using System;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Specification
{
    public partial class AbstractSpecificationTest
    {
        #region Helper classes

        public class TestEntity { }

        class TestSpecification : AbstractSpecification<TestEntity>
        {
            private bool Result { get; set; }

            public TestSpecification(bool isSatisfiedBy)
                : base((entity) => new Exception())
            {
                Result = isSatisfiedBy;
            }

            public override bool IsSatisfiedBy(TestEntity entity)
            {
                return Result;
            }
        }

        #endregion

        protected static ISpecification<TestEntity> GetSubjectUnderTest(bool isSatisfiedBy = true)
        {
            return new TestSpecification(isSatisfiedBy);
        }
    }
}
