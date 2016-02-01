#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Configuration.Extensions.Test
{
    public partial class EntryTest
    {
        [TestClass]
        public class EntryGetExceptionPolicyEntryTest
        {
            [TestMethod, UnitTest]
            public void GetExceptionPolicyEntry_Returns_An_Instance_Of_ExceptionPolicyEntry_With_ExceptionType_As_Specified()
            {
                Assert.AreEqual(
                    typeof(TestException),
                    GetExceptionPolicyEntry<TestException>(
                        It.IsAny<PostHandlingAction>()).ExceptionType);
            }

            [TestMethod, UnitTest]
            public void GetExceptionPolicyEntry_Returns_An_Instance_Of_ExceptionPolicyEntry_With_Action_As_None_When_Then_Is_PostHandlingAction_None()
            {
                Assert.AreEqual(
                    PostHandlingAction.None,
                    GetExceptionPolicyEntry(PostHandlingAction.None)
                        .Action);
            }

            [TestMethod, UnitTest]
            public void GetExceptionPolicyEntry_Returns_An_Instance_Of_ExceptionPolicyEntry_With_Action_As_ShouldRethrow_When_Then_Is_PostHandlingAction_ShouldRethrow()
            {
                Assert.AreEqual(
                    PostHandlingAction.ShouldRethrow,
                    GetExceptionPolicyEntry(PostHandlingAction.ShouldRethrow).Action);
            }

            [TestMethod, UnitTest]
            public void GetExceptionPolicyEntry_Returns_An_Instance_Of_ExceptionPolicyEntry_With_Action_As_ThrowNewException_When_Then_Is_PostHandlingAction_ThrowNewException()
            {
                Assert.AreEqual(
                    PostHandlingAction.ThrowNewException,
                    GetExceptionPolicyEntry(PostHandlingAction.ThrowNewException).Action);
            }

            private static ExceptionPolicyEntry
                GetExceptionPolicyEntry(PostHandlingAction action)
            {
                return GetExceptionPolicyEntry<Exception>(action);
            }

            private static ExceptionPolicyEntry
                GetExceptionPolicyEntry<TException>(
                    PostHandlingAction action = PostHandlingAction.None)
                where TException : Exception
            {
                return Entry.For<TException>()
                    .Then(action)
                    .WithExceptionHandler(GetExceptionHandlerData())
                    .GetExceptionPolicyEntry();
            }
        }
    }
}
