#region Using

using C4rm4x.Tools.Utilities;

#endregion

namespace C4rm4x.WebApi.Validation.Test
{
    public partial class AbstractValidatorTest
    {
        private const string RuleSet_Rule = "Rule";
        private const string RuleSet_Conditional = "Conditional";
        private const string TestProperty = "TestProperty";
        private const string TestValue = "TestValue";

        #region Helper classes

        class TestClass
        {
            public TestClass(string testProperty)
            {
                TestProperty = testProperty;
            }

            public string TestProperty { get; set; }
        }

        class TestClassValidator : AbstractValidator<TestClass>
        {
            public TestClassValidator()
            {
                RuleSet(RuleSet_Rule, () =>
                {
                    RuleFor(x => x.TestProperty)
                        .Equal(TestValue, "TestProperty must be equal to TestValue");
                });

                RuleSet(RuleSet_Conditional, () =>
                {
                    RuleFor(x => x.TestProperty)
                        .MaximumLength(2).Unless(x => x.TestProperty.IsNullOrEmpty());
                });

                RuleFor(x => x.TestProperty)
                    .NotNull("TestProperty cannot be null");
            }
        }

        #endregion

        private static TestClassValidator GetValidator()
        {
            return new TestClassValidator();
        }
    }
}
