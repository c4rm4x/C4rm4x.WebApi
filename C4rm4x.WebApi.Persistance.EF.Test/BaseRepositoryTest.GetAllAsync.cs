#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Persistance.EF.Test
{
    public partial class BaseRepositoryTest
    {
        [TestClass]
        public class BaseRepositoryGetAllAsyncTest :
            BaseRepositoryFixture
        {
            [TestMethod, IntegrationTest]
            public async Task GetAllAsync_Retrieves_All_Entities_In_Database()
            {
                var entities = await _sut.GetAllAsync();

                Assert.IsNotNull(entities);
                Assert.IsTrue(entities.Any());
                Assert.AreEqual(10, entities.Count());

                foreach (var entity in entities)
                {
                    Assert.IsNotNull(entity);
                    Assert.AreEqual(entity.Id.ToString(), entity.Value);
                }
            }

            [TestMethod, IntegrationTest]
            public async Task GetAllAsync_Retrieves_Empty_Collection_When_No_Entity_In_Database_Fulfills_Predicate()
            {
                var entities = await _sut.GetAllAsync(e => e.Id < 0);

                Assert.IsNotNull(entities);
                Assert.IsFalse(entities.Any());
            }

            [TestMethod, IntegrationTest]
            public async Task GetAllAsync_Retrieves_A_Collection_With_All_Entities_In_Database_That_Fulfill_Predicate()
            {
                var entities = await _sut.GetAllAsync(e => e.Id > 0);

                Assert.IsNotNull(entities);
                Assert.IsTrue(entities.Any());

                foreach (var entity in entities)
                {
                    Assert.IsNotNull(entity);
                    Assert.IsTrue(entity.Id > 0);
                    Assert.AreEqual(entity.Id.ToString(), entity.Value);
                }
            }
        }
    }
}
