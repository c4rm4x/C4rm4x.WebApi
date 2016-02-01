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
        public class EntryWithExceptionHandlerTest
        {
            [TestMethod, UnitTest]
            public void WithExceptionHandler_Returns_An_Instance_Of_Entry()
            {
                var result = Entry.For<Exception>()
                    .Then(It.IsAny<PostHandlingAction>())
                    .WithExceptionHandler(GetExceptionHandlerData());

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(Entry));
            }

            [TestMethod, UnitTest]
            public void WithExceptionHandler_Returns_Same_Entry()
            {
                var entry = Entry.For<Exception>()
                    .Then(It.IsAny<PostHandlingAction>());

                Assert.AreSame(
                    entry,
                    entry.WithExceptionHandler(GetExceptionHandlerData()));
            }
        }
    }
}
