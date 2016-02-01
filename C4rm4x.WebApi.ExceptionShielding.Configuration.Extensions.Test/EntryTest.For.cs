#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Configuration.Extensions.Test
{
    public partial class EntryTest
    {
        [TestClass]
        public class EntryForTest
        {
            [TestMethod, UnitTest]
            public void For_Creates_A_New_Instance_Of_Enty_With_Exception_Speficied()
            {
                var result = Entry.For<TestException>();

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(Entry));
                Assert.AreEqual(typeof(TestException), result.ExceptionType);
            }
        }
    }
}
