#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Test
{
    public partial class ExceptionManagerTest
    {
        [TestClass]
        public class ExceptionManagerHandleExceptionTest
        {
            private const string PolicyName = "Policy";

            [TestMethod, UnitTest]
            [ExpectedException(typeof(InvalidOperationException))]
            public void HandleException_Throws_Exception_When_There_Is_No_Policy_Configured()
            {
                CreateSubjectUnderTest()
                    .HandleException(new Exception(), PolicyName);
            }

            [TestMethod, UnitTest]
            public void HandleException_Handles_Exception_Using_ExceptionPolicyDefinition_When_This_Policy_Is_Configured()
            {
                var exception = new Exception();
                var policyDefinition = GetExceptionPolicyDefinition(PolicyName, ObjectMother.Create<bool>());

                CreateSubjectUnderTest(policyDefinition)
                    .HandleException(exception, PolicyName);

                Mock.Get(policyDefinition)
                    .Verify(p => p.HandleException(exception), Times.Once);
            }

            [TestMethod, UnitTest]
            public void HandleException_Returns_True_When_ExceptionPolicyDefinition_That_Manages_This_Exception_Returns_True()
            {
                Assert.IsTrue(
                    CreateSubjectUnderTest(
                        GetExceptionPolicyDefinition(PolicyName, true))
                        .HandleException(new Exception(), PolicyName));
            }

            [TestMethod, UnitTest]
            public void HandleException_Returns_False_When_ExceptionPolicyDefinition_That_Manages_This_Exception_Returns_False()
            {
                Assert.IsFalse(
                    CreateSubjectUnderTest(
                        GetExceptionPolicyDefinition(PolicyName, false))
                        .HandleException(new Exception(), PolicyName));
            }

            private static ExceptionManager CreateSubjectUnderTest(
                params IExceptionPolicyDefinition[] policyDefinitions)
            {
                return new ExceptionManager(policyDefinitions);
            }

            private static IExceptionPolicyDefinition GetExceptionPolicyDefinition(
                string policyName,
                bool returns = true)
            {
                var mock = Mock.Of<IExceptionPolicyDefinition>();

                Mock.Get(mock)
                    .SetupGet(p => p.PolicyName)
                    .Returns(policyName);

                Mock.Get(mock)
                    .Setup(p => p.HandleException(It.IsAny<Exception>()))
                    .Returns(returns);

                return mock;
            }
        }
    }
}
