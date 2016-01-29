#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.ExceptionShielding.Handlers;
using C4rm4x.WebApi.Framework.Log;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Test.Handlers
{
    public partial class LogHandlerTest
    {
        [TestClass]
        public class LogHandlerHandleTest
        {
            private const string Message = "{handlingInstanceID}";

            [TestMethod, UnitTest]
            public void Handle_Returns_Same_Exception()
            {
                var exception = new Exception();

                Assert.AreSame(
                    exception,
                    CreateSubjectUnderTest()
                        .HandleException(exception, Guid.NewGuid()));
            }

            [TestMethod, UnitTest]
            public void Handle_Logs_Message_As_Error_When_Severity_Is_Error()
            {
                var log = GetLog();

                CreateSubjectUnderTest(severity: Severity.Error, logger: log)
                    .HandleException(new Exception(), Guid.NewGuid());

                Mock.Get(log)
                    .Verify(l => l.Error(It.IsAny<string>()), Times.Once);
            }

            [TestMethod, UnitTest]
            public void Handle_Does_Not_Log_Message_As_Info_When_Severity_Is_Error()
            {
                var log = GetLog();

                CreateSubjectUnderTest(severity: Severity.Error, logger: log)
                    .HandleException(new Exception(), Guid.NewGuid());

                Mock.Get(log)
                    .Verify(l => l.Info(It.IsAny<string>()), Times.Never);
            }

            [TestMethod, UnitTest]
            public void Handle_Does_Not_Log_Message_As_Debug_When_Severity_Is_Error()
            {
                var log = GetLog();

                CreateSubjectUnderTest(severity: Severity.Error, logger: log)
                    .HandleException(new Exception(), Guid.NewGuid());

                Mock.Get(log)
                    .Verify(l => l.Debug(It.IsAny<string>()), Times.Never);
            }

            [TestMethod, UnitTest]
            public void Handle_Logs_Message_As_Info_When_Severity_Is_Info()
            {
                var log = GetLog();

                CreateSubjectUnderTest(severity: Severity.Info, logger: log)
                    .HandleException(new Exception(), Guid.NewGuid());

                Mock.Get(log)
                    .Verify(l => l.Info(It.IsAny<string>()), Times.Once);
            }

            [TestMethod, UnitTest]
            public void Handle_Does_Not_Log_Message_As_Error_When_Severity_Is_Info()
            {
                var log = GetLog();

                CreateSubjectUnderTest(severity: Severity.Info, logger: log)
                    .HandleException(new Exception(), Guid.NewGuid());

                Mock.Get(log)
                    .Verify(l => l.Error(It.IsAny<string>()), Times.Never);
            }

            [TestMethod, UnitTest]
            public void Handle_Does_Not_Log_Message_As_Debug_When_Severity_Is_Info()
            {
                var log = GetLog();

                CreateSubjectUnderTest(severity: Severity.Info, logger: log)
                    .HandleException(new Exception(), Guid.NewGuid());

                Mock.Get(log)
                    .Verify(l => l.Debug(It.IsAny<string>()), Times.Never);
            }

            [TestMethod, UnitTest]
            public void Handle_Logs_Message_As_Debug_When_Severity_Is_Debug()
            {
                var log = GetLog();

                CreateSubjectUnderTest(severity: Severity.Debug, logger: log)
                    .HandleException(new Exception(), Guid.NewGuid());

                Mock.Get(log)
                    .Verify(l => l.Debug(It.IsAny<string>()), Times.Once);
            }

            [TestMethod, UnitTest]
            public void Handle_Does_Not_Log_Message_As_Error_When_Severity_Is_Debug()
            {
                var log = GetLog();

                CreateSubjectUnderTest(severity: Severity.Debug, logger: log)
                    .HandleException(new Exception(), Guid.NewGuid());

                Mock.Get(log)
                    .Verify(l => l.Error(It.IsAny<string>()), Times.Never);
            }

            [TestMethod, UnitTest]
            public void Handle_Does_Not_Log_Message_As_Info_When_Severity_Is_Debug()
            {
                var log = GetLog();

                CreateSubjectUnderTest(severity: Severity.Debug, logger: log)
                    .HandleException(new Exception(), Guid.NewGuid());

                Mock.Get(log)
                    .Verify(l => l.Info(It.IsAny<string>()), Times.Never);
            }

            [TestMethod, UnitTest]
            public void Handle_Logs_Messasge_With_HandlingInstanceId_When_Token_Is_Present_In_ErrorMessageFormat()
            {
                var HandlingInstanceID = ObjectMother.Create<Guid>();
                var log = GetLog();

                CreateSubjectUnderTest(logger: log)
                    .HandleException(new Exception(), HandlingInstanceID);

                Mock.Get(log)
                    .Verify(l => l.Error(
                        It.Is<string>(s => s.StartsWith(HandlingInstanceID.ToString()))),
                        Times.Once);
            }

            [TestMethod, UnitTest]
            public void Handle_Logs_Messasge_Without_HandlingInstanceId_When_Token_Is_Not_Present_In_ErrorMessageFormat()
            {
                var HandlingInstanceID = ObjectMother.Create<Guid>();
                var OtherFormat = ObjectMother.Create<string>();
                var log = GetLog();

                CreateSubjectUnderTest(errorMessageFormat: OtherFormat, logger: log)
                    .HandleException(new Exception(), HandlingInstanceID);

                Mock.Get(log)
                    .Verify(l => l.Error(
                        It.Is<string>(s => s.StartsWith(HandlingInstanceID.ToString()))),
                        Times.Never);
            }

            private static LogHandler CreateSubjectUnderTest(
                Severity severity = Severity.Error,
                string errorMessageFormat = Message,
                ILog logger = null)
            {
                return new LogHandler(
                    severity,
                    errorMessageFormat,
                    logger ?? GetLog());
            }

            private static ILog GetLog()
            {
                return Mock.Of<ILog>();
            }
        }
    }
}
