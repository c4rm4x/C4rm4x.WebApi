#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.Specification;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Builders
{
    public class RuleBuilder
    {
        private readonly Rule _entity;

        public RuleBuilder()
        {
            _entity = new Rule(
                ObjectMother.Create<string>(),
                ObjectMother.Create<string>());
        }

        public Rule Build()
        {
            return _entity;
        }
    }
}
