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
        public class BaseRepositoryAddAsyncTest : BaseRepositoryFixture
        {
            [TestMethod, IntegrationTest]
            public async Task AddAsync_Adds_New_Entity_In_Context()
            {
                var entity = new TestTable(Value);

                await _sut.AddAsync(entity);

                Assert.AreEqual(EntityState.Added, GetEntityState(entity));
            }
        }
    }
}
