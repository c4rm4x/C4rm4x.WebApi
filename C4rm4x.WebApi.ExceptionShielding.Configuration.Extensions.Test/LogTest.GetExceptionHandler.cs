#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.ExceptionShielding.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Configuration.Extensions.Test
{
    public partial class LogTest
    {
        [TestClass]
        public class LogGetExceptionHandlerTest
        {
            [TestMethod, UnitTest]
            public void GetExceptionHandler_Returns_An_Instance_Of_LogHandler()
            {
                var result = Log.WithFormat(Format)
                    .Using(GetLog())
                    .GetExceptionHandler();

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(LogHandler));
            }
        }
    }
}
