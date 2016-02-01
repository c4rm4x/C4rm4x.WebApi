#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Configuration.Extensions.Test
{
    public partial class DefinitionTest
    {
        [TestClass]
        public class DefinitionWithExceptionPolicyEntryTest
        {
            private const string PolicyName = "PolicyName";

            [TestMethod, UnitTest]
            public void WithExceptionPolicyEntry_Returns_An_Instance_Of_Definition()
            {
                var result = Definition.WithName(PolicyName)
                    .WithExceptionPolicyEntry(GetEntry());

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(Definition));
            }

            [TestMethod, UnitTest]
            public void WithExceptionHandler_Returns_Same_Definition()
            {
                var definition = Definition.WithName(PolicyName);

                Assert.AreSame(
                    definition,
                    definition.WithExceptionPolicyEntry(GetEntry()));
            }
        }
    }
}
