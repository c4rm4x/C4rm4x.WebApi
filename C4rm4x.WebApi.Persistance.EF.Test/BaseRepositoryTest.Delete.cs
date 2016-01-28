#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.Persistance;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;

#endregion

namespace C4rm4x.WebApi.Persistance.EF.Test
{
    public partial class BaseRepositoryTest
    {
        [TestClass]
        public class BaseRepositoryDeleteTest :
            BaseRepositoryFixture
        {
            [TestMethod, IntegrationTest]
            public void Delete_Removes_Entity_From_Context()
            {
                var entity = _sut.Get(1);

                _sut.Delete(entity);

                Assert.AreEqual(EntityState.Deleted, GetEntityState(entity));
            }

            [TestMethod, IntegrationTest]
            [ExpectedException(typeof(PersistenceException))]
            public void Delete_Throws_PersistenceException_When_Entity_Cannot_Be_Found()
            {
                _sut.Delete(-1);
            }

            [TestMethod, IntegrationTest]
            public void Delete_Removes_Entity_From_Context_When_Entity_Is_In_Database()
            {
                const int EntityId = 1;

                _sut.Delete(EntityId);

                Assert.AreEqual(EntityState.Deleted, GetEntityState(_sut.Get(EntityId)));
            }
        }
    }
}
