#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.ExceptionShielding.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Configuration.Extensions.Test
{
    public partial class WrapTest
    {
        [TestClass]
        public class WrapWithTest
        {
            private const string Message = "Message";

            [TestMethod, UnitTest]
            public void With_Returns_An_Instance_Of_IExceptionHandlerData()
            {
                var result = Wrap.With<Exception>(Message);

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(IExceptionHandlerData));
            }

            [TestMethod, UnitTest]
            public void With_Returns_An_Instance_Of_IExceptionHandlerData_Which_Generates_An_Instance_Of_WrapeHandler()
            {
                var result = Wrap.With<Exception>(Message)
                    .GetExceptionHandler();

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(WrapHandler));
            }
        }
    }
}
