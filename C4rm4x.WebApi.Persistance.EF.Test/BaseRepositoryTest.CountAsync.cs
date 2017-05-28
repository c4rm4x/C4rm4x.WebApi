#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Persistance.EF.Test
{
    public partial class BaseRepositoryTest
    {
        [TestClass]
        public class BaseRepositoryCountAsyncTest :
            BaseRepositoryFixture
        {
            [TestMethod, IntegrationTest]
            public async Task CountAsync_Returns_The_Total_Number_Of_Entities_In_Database()
            {
                Assert.AreEqual(10, await _sut.CountAsync());
            }

            [TestMethod, IntegrationTest]
            public async Task CountAsync_Returns_0_When_No_Entity_In_Database_Fulfills_Predicate()
            {
                Assert.AreEqual(0, await _sut.CountAsync(e => e.Id < 0));
            }

            [TestMethod, IntegrationTest]
            public async Task CountAsync_Returns_The_Total_Number_Of_Entities_In_Database_That_Fulfill_Predicate()
            {
                Assert.AreEqual(10, await _sut.CountAsync(e => e.Id > 0));
            }
        }
    }
}
