#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Configuration.Test
{
    public partial class ThenTest
    {
        [TestClass]
        public class ThenDoNothingTest
        {
            [TestMethod, UnitTest]
            public void DoNothing_Returns_PostHandlingAction_None()
            {
                Assert.AreEqual(
                    PostHandlingAction.None,
                    Then.DoNothing());
            }
        }
    }
}
