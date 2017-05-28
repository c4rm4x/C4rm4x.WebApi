﻿#region Using

using C4rm4x.WebApi.Framework.Specification;
using C4rm4x.WebApi.Framework.Test.Builders;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Specification
{
    public partial class AndSpecificationTest
    {
        #region Helper classes

        public class TestEntity { }

        class TestSpecification : ExpressionSpecification<TestEntity>
        {
            public TestSpecification(bool isSatisfiedBy)
                : base(
                      new RuleBuilder().Build(),
                      (entity) => isSatisfiedBy)
            {
            }
        }

        class TestAndSpecification : AndSpecification<TestEntity>
        {
            public TestAndSpecification(
                bool isLeftSatisfiedBy,
                bool isRightSatisfiedBy)
                : base(
                      new RuleBuilder().Build(),
                      new TestSpecification(isLeftSatisfiedBy),
                      new TestSpecification(isRightSatisfiedBy))
            {
            }
        }

        #endregion

        protected static ISpecification<TestEntity> GetSubjectUnderTest(
            bool isLeftSatisfiedBy = true,
            bool isRightSatisfiedBy = true)
        {
            return new TestAndSpecification(isLeftSatisfiedBy, isRightSatisfiedBy);
        }
    }
}
