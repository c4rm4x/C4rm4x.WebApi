#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Configuration.Extensions.Test
{
    public partial class ExceptionManagerBuilderTest
    {
        [TestClass]
        public class ExceptionManagerBuilderConfigureTest
        {
            [TestMethod, UnitTest]
            public void Configure_Creates_A_New_Instance_Of_ExceptionManagerBuilder()
            {
                var result = ExceptionManagerBuilder.Configure();

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(ExceptionManagerBuilder));
            }
        }
    }
}
