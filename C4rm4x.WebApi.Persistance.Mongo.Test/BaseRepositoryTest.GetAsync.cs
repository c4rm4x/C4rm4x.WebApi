#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Persistance.Mongo.Test
{
    public partial class BaseRepositoryTest
    {
        [TestClass]
        public class BaseRepositoryGetAsyncTest :
            BaseRepositoryFixture
        {
            [TestMethod, IntegrationTest]
            public async Task GetAsync_Retrieves_Null_When_No_Entity_In_Database_Fulfills_Predicate()
            {
                Assert.IsNull(await _sut.GetAsync(e => e.Value == "0"));
            }

            [TestMethod, IntegrationTest]
            public async Task GetAsync_Retrieves_First_Entity_That_Fulfills_Predicate()
            {
                var entity = await _sut.GetAsync(e => e.Value != "0");

                Assert.IsNotNull(entity);
                Assert.IsTrue(entity.Value != "0");
            }
        }
    }
}
