#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Configuration.Extensions.Test
{
    public partial class ExceptionManagerBuilderTest
    {
        [TestClass]
        public class ExceptionManagerBuilderWithExceptionPolicyDefinitionTest
        {
            [TestMethod, UnitTest]
            public void WithExceptionPolicyDefinition_Returns_An_Instance_Of_ExceptionManagerBuilder()
            {
                var result = ExceptionManagerBuilder.
                    Configure()
                    .WithExceptionPolicyDefinition(GetDefinition());

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(ExceptionManagerBuilder));
            }

            [TestMethod, UnitTest]
            public void WithExceptionHandler_Returns_Same_ExceptionManagerBuilder()
            {
                var exceptionManagerBuilder = ExceptionManagerBuilder.Configure();

                Assert.AreSame(
                    exceptionManagerBuilder,
                    exceptionManagerBuilder.WithExceptionPolicyDefinition(GetDefinition()));
            }
        }
    }
}
