#region Using

using C4rm4x.WebApi.Framework.Specification;
using System;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Specification
{
    public partial class ExpressionSpecificationTest
    {
        #region Helper classes

        public class TestEntity { }

        class TestExpressionSpecification : ExpressionSpecification<TestEntity>
        {
            public TestExpressionSpecification(
                bool isSatisfiedBy) : base(
                    (entity) => isSatisfiedBy, 
                    (entity) => new Exception())
            {
            }
        }

        #endregion

        protected static ISpecification<TestEntity> GetSubjectUnderTest(bool isSatisfiedBy = true)
        {
            return new TestExpressionSpecification(isSatisfiedBy);
        }
    }
}
