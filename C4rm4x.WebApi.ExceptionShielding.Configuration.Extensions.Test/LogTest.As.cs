#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.ExceptionShielding.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Configuration.Extensions.Test
{
    public partial class LogTest
    {
        [TestClass]
        public class LogAsTest
        {
            [TestMethod, UnitTest]
            public void As_Returns_An_Instance_Of_Log()
            {
                var result = Log.WithFormat(Format)
                    .As(It.IsAny<Severity>());

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(Log));
            }

            [TestMethod, UnitTest]
            public void As_Returns_Same_Log()
            {
                var log = Log.WithFormat(Format);

                Assert.AreSame(
                    log,
                    log.As(It.IsAny<Severity>()));
            }

            [TestMethod, UnitTest]
            public void As_Sets_Severity_As_Severity_Error_When_Value_Is_Severity_Error()
            {
                Assert.AreEqual(
                    Severity.Error,
                    Log.WithFormat(Format)
                        .As(Severity.Error)
                        .Severity);
            }

            [TestMethod, UnitTest]
            public void As_Sets_Severity_As_Severity_Info_When_Value_Is_Severity_Info()
            {
                Assert.AreEqual(
                    Severity.Info,
                    Log.WithFormat(Format)
                        .As(Severity.Info)
                        .Severity);
            }

            [TestMethod, UnitTest]
            public void As_Sets_Severity_As_Severity_Debug_When_Value_Is_Severity_Debug()
            {
                Assert.AreEqual(
                    Severity.Debug,
                    Log.WithFormat(Format)
                        .As(Severity.Debug)
                        .Severity);
            }
        }
    }
}
