#region Using

using Moq;
using System;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Configuration.Extensions.Test
{
    public partial class EntryTest
    {
        #region Helper classes

        private class TestException : Exception { }

        #endregion

        private static IExceptionHandlerData GetExceptionHandlerData()
        {
            return Mock.Of<IExceptionHandlerData>();
        }
    }
}
