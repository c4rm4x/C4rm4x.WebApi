#region Using

using C4rm4x.WebApi.Framework.Log;
using Moq;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Configuration.Test
{
    public partial class LogTest
    {
        private const string Format = "Format";

        private static ILog GetLog()
        {
            return Mock.Of<ILog>();
        }
    }
}
