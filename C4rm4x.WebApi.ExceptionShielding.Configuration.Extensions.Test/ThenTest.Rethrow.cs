#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Configuration.Extensions.Test
{
    public partial class ThenTest
    {
        [TestClass]
        public class ThenRethrowTest
        {
            [TestMethod, UnitTest]
            public void Rethrow_Returns_PostHandlingAction_ShouldRethrow()
            {
                Assert.AreEqual(
                    PostHandlingAction.ShouldRethrow,
                    Then.Rethrow());
            }
        }
    }
}
