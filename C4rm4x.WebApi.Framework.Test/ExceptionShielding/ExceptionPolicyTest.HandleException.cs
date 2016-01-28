#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.ExceptionShielding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

#endregion

namespace C4rm4x.WebApi.Framework.Test.ExceptionShielding
{
    public partial class ExceptionPolicyTest
    {
        private const string PolicyName = "PolicyName";

        #region Helper classes

        private class TestException : Exception { }

        #endregion

        [TestClass]
        public class ExceptionPoilcyHandleExceptionTest
        {
            [TestInitialize]
            public void Setup()
            {
                SetupExceptionExceptionManager(false);
            }

            [TestMethod, UnitTest]
            public void HandleException_Returns_True_When_ExceptionManager_Returns_True()
            {
                Exception exceptionToThrow;

                SetupExceptionExceptionManager(true);

                Assert.IsTrue(HandleException(out exceptionToThrow));
            }

            [TestMethod, UnitTest]
            public void HandleException_Sets_ExceptionToThrow_To_Null_When_ExceptionManager_Returns_True()
            {
                Exception exceptionToThrow;

                SetupExceptionExceptionManager(true);

                HandleException(out exceptionToThrow);

                Assert.IsNull(exceptionToThrow);
            }

            [TestMethod, UnitTest]
            public void HandleException_Returns_False_When_ExceptionManager_Returns_False()
            {
                Exception exceptionToThrow;

                Assert.IsFalse(HandleException(out exceptionToThrow));
            }

            [TestMethod, UnitTest]
            public void HandleException_Sets_ExceptionToThrow_To_Null_When_ExceptionManager_Returns_False()
            {
                Exception exceptionToThrow;

                HandleException(out exceptionToThrow);

                Assert.IsNull(exceptionToThrow);
            }

            [TestMethod, UnitTest]
            public void HandleException_Returns_True_When_ExceptionManager_Throws_Exception()
            {
                Exception exceptionToThrow;

                SetupExceptionExceptionManager(ObjectMother.Create<bool>(), true);

                Assert.IsTrue(HandleException(out exceptionToThrow));
            }

            [TestMethod, UnitTest]
            public void HandleException_Sets_ExceptionToThrow_With_The_Exception_Thrown_By_ExceptionManager_When_This_Does_So()
            {
                Exception exceptionToThrow;

                SetupExceptionExceptionManager(ObjectMother.Create<bool>(), true);

                HandleException(out exceptionToThrow);

                Assert.IsNotNull(exceptionToThrow);
                Assert.IsInstanceOfType(exceptionToThrow, typeof(TestException));
            }

            private static void SetupExceptionExceptionManager(
                bool returns,
                bool isExceptionManagerThrownException = false)
            {
                ExceptionPolicy.SetExceptionManager(
                    GetExceptionManager<TestException>(returns, isExceptionManagerThrownException));
            }

            private static IExceptionManager GetExceptionManager<TException>(
                bool returns,
                bool isExceptionManagerThrownException)
                where TException : Exception, new()
            {
                var exceptionManager = Mock.Of<IExceptionManager>();

                if (isExceptionManagerThrownException)
                    Mock.Get(exceptionManager)
                        .Setup(e => e.HandleException(It.IsAny<Exception>(), PolicyName))
                        .Throws<TException>();
                else
                    Mock.Get(exceptionManager)
                        .Setup(e => e.HandleException(It.IsAny<Exception>(), PolicyName))
                        .Returns(returns);

                return exceptionManager;
            }

            private static bool HandleException(out Exception exceptionToThrow)
            {
                return ExceptionPolicy.HandleException(
                    Mock.Of<Exception>(), PolicyName, out exceptionToThrow);
            }
        }
    }
}
