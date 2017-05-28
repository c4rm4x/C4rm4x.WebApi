#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Persistance.EF.Test.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Persistance.EF.Test
{
    public partial class BaseRepositoryTest
    {
        [TestClass]
        public class BaseRepositoryUpdateAsyncTest :
            BaseRepositoryFixture
        {
            [TestMethod, IntegrationTest]
            public async Task UpdateAsync_Sets_Entity_State_as_Modified()
            {
                var entity = new TestTable(Value);

                await _sut.UpdateAsync(entity);

                Assert.AreEqual(EntityState.Modified, GetEntityState(entity));
            }
        }
    }
}
