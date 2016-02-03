#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Configuration.Test
{
    public partial class ThenTest
    {
        [TestClass]
        public class ThenThrowNewExceptionTest
        {
            [TestMethod, UnitTest]
            public void ThrowNewException_Returns_PostHandlingAction_ThrowNewException()
            {
                Assert.AreEqual(
                    PostHandlingAction.ThrowNewException,
                    Then.ThrowNewException());
            }
        }
    }
}
