#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.ExceptionShielding.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Test.Handlers
{
    public partial class ReplaceHandlerTest
    {
        [TestClass]
        public class ReplaceHandlerHandleTest
        {
            private const string Message = "{handlingInstanceID}";

            #region Helper classes

            internal class TestException : Exception
            {
                public TestException(string message)
                    : base(message)
                {
                }
            }

            #endregion

            [TestMethod, UnitTest]
            public void Handle_Returns_An_Exception_Of_Type_TException()
            {
                Assert.IsInstanceOfType(
                    CreateSubjectUnderTest<TestException>()
                        .HandleException(new Exception(), Guid.NewGuid()),
                        typeof(TestException));
            }

            [TestMethod, UnitTest]
            public void Handle_Returns_An_Exception_With_Messasge_With_HandlingInstanceId_When_Token_Is_Present()
            {
                var HandlingInstanceID = ObjectMother.Create<Guid>();

                Assert.AreEqual(
                    HandlingInstanceID.ToString(),
                    CreateSubjectUnderTest()
                        .HandleException(new Exception(), HandlingInstanceID)
                        .Message);
            }

            [TestMethod, UnitTest]
            public void Handle_Returns_An_Exception_With_Message_Without_HandlingInstanceId_When_Token_Is_Not_Present()
            {
                var Message = ObjectMother.Create<string>();

                Assert.AreEqual(Message,
                    CreateSubjectUnderTest(Message)
                        .HandleException(new Exception(), Guid.NewGuid())
                        .Message);
            }

            private static ReplaceHandler CreateSubjectUnderTest(
                string message = Message)
            {
                return CreateSubjectUnderTest<Exception>(message);
            }

            private static ReplaceHandler CreateSubjectUnderTest<TException>(
                string message = Message)
                where TException : Exception
            {
                return new ReplaceHandler(message, typeof(TException));
            }
        }
    }
}
