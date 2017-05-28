#region Using

using C4rm4x.WebApi.Framework.Specification;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Specification
{
    public partial class OrSpecificationTest
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

            public async Task<bool> IsSatisfiedByAsync(TestEntity entity)
            {
                return await Task.FromResult(IsValid);
            }
        }

        class TestOrSpecification : OrSpecification<TestEntity>
        {
            public TestOrSpecification(
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
            return new TestOrSpecification(isLeftSatisfiedBy, isRightSatisfiedBy);
        }
    }
}
