#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Configuration.Extensions.Test
{
    public partial class DefinitionTest
    {
        [TestClass]
        public class DefinitionGetExceptionPolicyDefinitionTest
        {
            [TestMethod, UnitTest]
            public void GetExceptionPolicyDefinition_Returns_An_Instance_Of_ExceptionPolicyDefinition_With_PolicyName_As_Specified()
            {
                var PolicyName = ObjectMother.Create<string>();

                Assert.AreEqual(
                    PolicyName,
                    GetExceptionPolicyDefinition(PolicyName).PolicyName);
            }

            private static ExceptionPolicyDefinition
                GetExceptionPolicyDefinition(string policyName)
            {
                return Definition.WithName(policyName)
                    .WithExceptionPolicyEntry(GetEntry())
                    .GetExceptionPolicyDefinition();
            }
        }
    }
}
