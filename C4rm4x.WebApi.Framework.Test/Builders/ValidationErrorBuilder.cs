#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.Validation;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Builders
{
    public class ValidationErrorBuilder
    {
        private ValidationError _entity;

        public ValidationErrorBuilder()
        {
            _entity = new ValidationError(
                ObjectMother.Create(10),
                null,
                ObjectMother.Create(100));
        }

        public ValidationError Build()
        {
            return _entity;
        }
    }
}
