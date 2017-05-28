#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Persistance.EF.Test
{
    public partial class BaseRepositoryTest
    {
        [TestClass]
        public class BaseRepositoryDeleteAsyncTest :
            BaseRepositoryFixture
        {
            [TestMethod, IntegrationTest]
            public void DeleteAsync_Removes_Entity_From_Context()
            {
                var entity = _sut.GetAsync(1).Result;

                _sut.DeleteAsync(entity).Wait(); // For testing

                Assert.AreEqual(EntityState.Deleted, GetEntityState(entity));
            }

            [TestMethod, IntegrationTest]
            public void DeleteAsync_Removes_Entity_From_Context_When_Entity_Is_In_Database()
            {
                const int EntityId = 1;

                _sut.DeleteAsync(EntityId).Wait(); // For testing

                Assert.AreEqual(EntityState.Deleted, GetEntityState(_sut.GetAsync(EntityId).Result));
            }
        }
    }
}
