#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Persistance.EF.Test.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;

#endregion

namespace C4rm4x.WebApi.Persistance.EF.Test
{
    public partial class BaseRepositoryTest
    {
        [TestClass]
        public class BaseRepositoryUpdateTest :
            BaseRepositoryFixture
        {
            [TestMethod, IntegrationTest]
            public void Update_Sets_Entity_State_as_Modified()
            {
                var entity = new TestTable(Value);

                _sut.Update(entity);

                Assert.AreEqual(EntityState.Modified, GetEntityState(entity));
            }
        }
    }
}
