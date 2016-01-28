#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Persistance.EF.Test.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;

#endregion

namespace C4rm4x.WebApi.Persistance.EF.Test
{
    public partial class UnitOfWorkTest
    {
        [TestClass]
        public class UnitOfWorkRollbackTest : UnitOfWorkFixture
        {
            [TestMethod, IntegrationTest]
            public void Rollback_Detaches_All_Added_Entities()
            {
                var entity = new TestTable(Value);

                SetEntityState(entity, EntityState.Added);

                Assert.AreEqual(EntityState.Added, GetEntityState(entity));

                _sut.Rollback();

                Assert.AreEqual(EntityState.Detached, GetEntityState(entity));
            }

            [TestMethod, IntegrationTest]
            public void Rollback_Unchanges_All_Deleted_Entities()
            {
                var entity = SavesEntity();

                SetEntityState(entity, EntityState.Deleted);

                Assert.AreEqual(EntityState.Deleted, GetEntityState(entity));

                _sut.Rollback();

                Assert.AreEqual(EntityState.Unchanged, GetEntityState(entity));
            }

            [TestMethod, IntegrationTest]
            public void Rollback_Unchanges_All_Modified_Entities()
            {
                var entity = SavesEntity();

                SetEntityState(entity, EntityState.Modified);

                _sut.Rollback();

                Assert.AreEqual(EntityState.Unchanged, GetEntityState(entity));
            }

            [TestMethod, IntegrationTest]
            public void Rollback_Overwrites_All_Modified_Entities_With_Original_Values()
            {
                var entity = SavesEntity();

                entity.Value = "New Value";
                SetEntityState(entity, EntityState.Modified);

                _sut.Rollback();

                Assert.AreEqual(Value, entity.Value);
            }

            private TestTable SavesEntity()
            {
                var entity = new TestTable(Value);

                SetEntityState(entity, EntityState.Added);

                _sut.Commit();

                return entity;
            }
        }
    }
}
