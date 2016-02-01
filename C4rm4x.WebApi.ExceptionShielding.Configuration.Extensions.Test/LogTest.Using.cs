#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Configuration.Extensions.Test
{
    public partial class LogTest
    {
        [TestClass]
        public class LogUsingTest
        {
            [TestMethod, UnitTest]
            public void Using_Returns_An_Instance_Of_Log()
            {
                var result = Log.WithFormat(Format)
                    .Using(GetLog());

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(Log));
            }

            [TestMethod, UnitTest]
            public void Using_Returns_Same_Log()
            {
                var log = Log.WithFormat(Format);

                Assert.AreSame(
                    log,
                    log.Using(GetLog()));
            }
        }
    }
}
