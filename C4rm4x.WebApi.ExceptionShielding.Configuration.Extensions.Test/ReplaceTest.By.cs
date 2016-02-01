#region using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.ExceptionShielding.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Configuration.Extensions.Test
{
    public partial class ReplaceTest
    {
        [TestClass]
        public class ReplaceByTest
        {
            private const string Message = "Message";

            [TestMethod, UnitTest]
            public void By_Returns_An_Instance_Of_IExceptionHandlerData()
            {
                var result = Replace.By<Exception>(Message);

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(IExceptionHandlerData));
            }

            [TestMethod, UnitTest]
            public void By_Returns_An_Instance_Of_IExceptionHandlerData_Which_Generates_An_Instance_Of_ReplaceHandler()
            {
                var result = Replace.By<Exception>(Message)
                    .GetExceptionHandler();

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(ReplaceHandler));
            }
        }
    }
}
