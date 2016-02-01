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
        public class EntryThenTest
        {
            [TestMethod, UnitTest]
            public void Then_Returns_An_Instance_Of_Entry()
            {
                var result = Entry.For<Exception>()
                    .Then(It.IsAny<PostHandlingAction>());

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(Entry));
            }

            [TestMethod, UnitTest]
            public void Then_Returns_Same_Entry()
            {
                var entry = Entry.For<Exception>();

                Assert.AreSame(
                    entry,
                    entry.Then(It.IsAny<PostHandlingAction>()));
            }

            [TestMethod, UnitTest]
            public void Then_Sets_Action_As_PostHandling_None_When_Value_Is_PostHandling_None()
            {
                Assert.AreEqual(
                    PostHandlingAction.None,
                    Entry.For<Exception>()
                        .Then(PostHandlingAction.None)
                        .Action);
            }

            [TestMethod, UnitTest]
            public void Then_Sets_Action_As_PostHandling_ShouldRethrow_When_Value_Is_PostHandling_ShouldRethrow()
            {
                Assert.AreEqual(
                    PostHandlingAction.ShouldRethrow,
                    Entry.For<Exception>()
                        .Then(PostHandlingAction.ShouldRethrow)
                        .Action);
            }

            [TestMethod, UnitTest]
            public void Then_Sets_Action_As_PostHandling_ThrowNewException_When_Value_Is_PostHandling_ThrowNewException()
            {
                Assert.AreEqual(
                    PostHandlingAction.ThrowNewException,
                    Entry.For<Exception>()
                        .Then(PostHandlingAction.ThrowNewException)
                        .Action);
            }
        }
    }
}
