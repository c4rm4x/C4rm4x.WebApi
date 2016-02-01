#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Configuration.Extensions.Test
{
    public partial class DefinitionTest
    {
        [TestClass]
        public class DefinitionNameTest
        {
            [TestMethod, UnitTest]
            public void WithName_Creates_A_New_Instance_Of_Definition_With_PolicyName_Speficied()
            {
                var PolicyName = ObjectMother.Create<string>();
                var result = Definition.WithName(PolicyName);

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(Definition));
                Assert.AreEqual(PolicyName, result.PolicyName);
            }
        }
    }
}
