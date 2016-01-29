#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Test
{
    public partial class ExceptionPolicyDefinitionTest
    {
        [TestClass]
        public class ExceptionPolicyDefinitionHandleExceptionTest
        {
            private const string PolicyName = "Policy";

            [TestMethod, UnitTest]
            public void HandleException_Returns_True_When_No_ExceptionPolicyEntry_Is_Configured_For_Such_Exception()
            {
                Assert.IsTrue(
                    CreateSubjectUnderTest()
                    .HandleException(new Exception()));
            }

            [TestMethod, UnitTest]
            public void HandleException_Handles_Exception_Using_ExceptionPolicyEntry_When_This_Exception_Type_Is_Configured()
            {
                var exception = new Exception();
                var policyEntry = GetExceptionPolicyEntry<Exception>(ObjectMother.Create<bool>());

                CreateSubjectUnderTest(policyEntry)
                    .HandleException(exception);

                Mock.Get(policyEntry)
                    .Verify(e => e.Handle(exception), Times.Once);
            }

            [TestMethod, UnitTest]
            public void HandleException_Handles_Exception_Using_ExceptionPolicyEntry_When_This_Exception_Subtype_Is_Configured()
            {
                var exception = new ArgumentException();
                var policyEntry = GetExceptionPolicyEntry<Exception>(ObjectMother.Create<bool>());

                CreateSubjectUnderTest(policyEntry)
                    .HandleException(exception);

                Mock.Get(policyEntry)
                    .Verify(e => e.Handle(exception), Times.Once);
            }

            [TestMethod, UnitTest]
            public void HandleException_Returns_True_When_ExceptionPolicyEntry_That_Manages_This_Exception_Returns_True()
            {
                Assert.IsTrue(
                    CreateSubjectUnderTest(
                        GetExceptionPolicyEntry<Exception>(true))
                        .HandleException(new Exception()));
            }

            [TestMethod, UnitTest]
            public void HandleException_Returns_False_When_ExceptionPolicyEntry_That_Manages_This_Exception_Returns_False()
            {
                Assert.IsFalse(
                    CreateSubjectUnderTest(
                        GetExceptionPolicyEntry<Exception>(false))
                        .HandleException(new Exception()));
            }

            private static ExceptionPolicyDefinition CreateSubjectUnderTest(
                params IExceptionPolicyEntry[] policyEntries)
            {
                return new ExceptionPolicyDefinition(PolicyName, policyEntries);
            }

            private static IExceptionPolicyEntry GetExceptionPolicyEntry<TException>(
                bool returns = false)
                where TException : Exception
            {
                var mock = Mock.Of<IExceptionPolicyEntry>();

                Mock.Get(mock)
                    .SetupGet(e => e.ExceptionType)
                    .Returns(typeof(TException));

                Mock.Get(mock)
                    .Setup(e => e.Handle(It.IsAny<Exception>()))
                    .Returns(returns);

                return mock;
            }
        }
    }
}
