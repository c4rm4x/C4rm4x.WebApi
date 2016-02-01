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
        public class LogWithFormatTest
        {
            [TestMethod, UnitTest]
            public void WithFormat_Creates_A_New_Instance_Of_Log_With_Format_Speficied()
            {
                var Format = ObjectMother.Create<string>();

                var result = Log.WithFormat(Format);

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(Log));
                Assert.AreEqual(Format, result.ErrorMessageFormat);
            }

            [TestMethod, UnitTest]
            public void WithFormat_Creates_A_New_Instance_Of_Log_With_Severity_Error_As_Severity()
            {
                Assert.AreEqual(
                    Severity.Error,
                    Log.WithFormat(Format).Severity);
            }
        }
    }
}
