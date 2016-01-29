#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Test
{
    public partial class ExceptionPolicyEntryTest
    {
        [TestClass]
        public class ExceptionPolicyEntryHandleTest
        {
            #region Helper classes

            internal class TestException : Exception { }

            #endregion

            [TestMethod, UnitTest]
            public void Handle_Iterates_Through_All_Handlers_For_The_Entry()
            {
                var handlers = GetExceptionHandlerMock(GetRand(10)).ToArray();

                CreateSubjectUnderTest(
                    It.IsAny<PostHandlingAction>(),
                    handlers)
                    .Handle(new Exception());

                foreach (var handler in handlers)
                    Mock.Get(handler)
                        .Verify(h => h.HandleException(It.IsAny<Exception>(), It.IsAny<Guid>()), Times.Once);
            }

            [TestMethod, UnitTest]
            public void Handle_Returns_False_When_Action_Is_PostHandlingAction_None()
            {
                Assert.IsFalse(
                    CreateSubjectUnderTest(
                        PostHandlingAction.None,
                        CreateExceptionHandler())
                        .Handle(new Exception()));
            }

            [TestMethod, UnitTest]
            public void Handle_Returns_True_When_Action_Is_PostHandlingAction_ShouldRethrow()
            {
                Assert.IsTrue(
                    CreateSubjectUnderTest(
                        PostHandlingAction.ShouldRethrow,
                        CreateExceptionHandler())
                        .Handle(new Exception()));
            }

            [TestMethod, UnitTest]
            [ExpectedException(typeof(TestException))]
            public void Handle_Throws_Exception_Of_Type_Of_Last_ExceptionHandler_In_The_Chain_When_Action_Is_PostHandlingAction_ThrowNewException()
            {
                CreateSubjectUnderTest(
                    PostHandlingAction.ThrowNewException,
                    CreateExceptionHandler<TestException>())
                    .Handle(new Exception());
            }

            private static ExceptionPolicyEntry CreateSubjectUnderTest(
                PostHandlingAction action = PostHandlingAction.None,
                params IExceptionHandler[] handlers)
            {
                return CreateSubjectUnderTest<Exception>(action, handlers);
            }

            private static ExceptionPolicyEntry CreateSubjectUnderTest<TException>(
                PostHandlingAction action = PostHandlingAction.None,
                params IExceptionHandler[] handlers)
                where TException : Exception
            {
                return new ExceptionPolicyEntry(
                    typeof(TException),
                    action,
                    handlers);
            }

            private static IExceptionHandler CreateExceptionHandler()
            {
                return CreateExceptionHandler<Exception>();
            }

            private static IExceptionHandler
                CreateExceptionHandler<TException>()
                where TException : Exception, new()
            {
                var mock = Mock.Of<IExceptionHandler>();

                Mock.Get(mock)
                    .Setup(h => h.HandleException(It.IsAny<Exception>(), It.IsAny<Guid>()))
                    .Returns(new TException());

                return mock;
            }

            private IEnumerable<IExceptionHandler> GetExceptionHandlerMock(
                int numberOfExceptionHandlerMocks)
            {
                for (int i = 0; i < numberOfExceptionHandlerMocks; i++)
                    yield return CreateExceptionHandler();
            }

            private static int GetRand(int max)
            {
                return new Random().Next(1, max);
            }
        }
    }
}
