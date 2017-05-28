#region Using

using C4rm4x.WebApi.Framework.Specification;
using C4rm4x.WebApi.Framework.Validation;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Specification
{
    public partial class AndSpecificationTest
    {
        #region Helper classes

        public class TestEntity { }

        class TestSpecification : ISpecification<TestEntity>
        {
            public bool IsValid { get; private set; }

            public TestSpecification(bool isValid)                
            {
                IsValid = isValid;
            }

            public async Task<bool> IsSatisfiedByAsync(
                TestEntity entity, 
                ICollection<ValidationError> errors)
            {
                return await Task.FromResult(IsValid);
            }
        }

        class TestAndSpecification : AndSpecification<TestEntity>
        {
            public TestAndSpecification(
                bool isLeftSatisfiedBy,
                bool isRightSatisfiedBy)
                : base(
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
