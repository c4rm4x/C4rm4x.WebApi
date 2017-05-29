#region Using

using C4rm4x.WebApi.Framework.Validation;
using C4rm4x.WebApi.Validation.Core;
using C4rm4x.WebApi.Validation.Validators;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Validation.Test.Validators
{
    public abstract class AbstractValidatorTest<T>
            where T : IPropertyValidator
    {
        protected async Task<IEnumerable<ValidationError>> ValidateAsync(object value)
        {
            var sut = CreateSubjectUnderTest();

            return await sut.ValidateAsync(new PropertyValidatorContext(
                new ValidationContext(new TestClass
                {
                    TestProperty = value,
                }),
                PropertyRule.Create<TestClass, object>((x) => x.TestProperty),
                "TestProperty"));
        }

        protected abstract T CreateSubjectUnderTest();
    }

    #region Helper classes

    public class TestClass
    {
        public object TestProperty { get; set; }
    }

    #endregion
}
