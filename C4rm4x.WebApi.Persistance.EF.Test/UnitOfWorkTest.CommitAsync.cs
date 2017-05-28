#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Persistance.EF.Test.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Persistance.EF.Test
{
    public partial class UnitOfWorkTest
    {
        [TestClass]
        public class UnitOfWorkCommitAsyncTest : UnitOfWorkFixture
        {
            [TestMethod, IntegrationTest]
            public async Task CommitAsync_Saves_Context_Into_Database()
            {
                var entity = new TestTable(Value);

                SetEntityState(entity, EntityState.Added);

                Assert.AreEqual(0, entity.Id);

                await _sut.CommitAsync();

                Assert.AreNotEqual(0, entity.Id);
            }
        }
    }
}
