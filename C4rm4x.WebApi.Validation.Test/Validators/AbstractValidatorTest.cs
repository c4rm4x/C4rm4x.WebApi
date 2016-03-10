#region Using

using C4rm4x.WebApi.Framework.Validation;
using C4rm4x.WebApi.Validation.Core;
using C4rm4x.WebApi.Validation.Validators;
using System.Collections.Generic;

#endregion

namespace C4rm4x.WebApi.Validation.Test.Validators
{
    public abstract class AbstractValidatorTest<T>
            where T : IPropertyValidator
    {
        protected IEnumerable<ValidationError> Validate(object value)
        {
            var sut = CreateSubjectUnderTest();

            return sut.Validate(new PropertyValidatorContext(
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
