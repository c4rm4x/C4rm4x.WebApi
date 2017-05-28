#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Runtime
{
    public partial class ExecutionContextTest
    {
        #region Helper classes

        public class TestExtension { }

        #endregion

        [TestClass]
        public abstract class ExecutionContextFixture
            : AutoMockFixture<ExecutionContext>
        { }
    }
}
