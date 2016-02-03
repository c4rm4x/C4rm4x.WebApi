#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Configuration.Test
{
    public partial class ExceptionManagerBuilderTest
    {
        [TestClass]
        public class ExceptionManagerBuilderBuildTest
        {
            [TestMethod, UnitTest]
            public void Build_Returns_An_Instance_Of_ExceptionManager()
            {
                Assert.IsNotNull(Build());
            }

            private static ExceptionManager Build()
            {
                return ExceptionManagerBuilder
                    .Configure()
                    .WithExceptionPolicyDefinition(GetDefinition())
                    .Build();
            }
        }
    }
}
