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
        public class BaseRepositoryAddTest : BaseRepositoryFixture
        {
            [TestMethod, IntegrationTest]
            public void Add_Adds_New_Entity_In_Context()
            {
                var entity = new TestTable(Value);

                _sut.Add(entity);

                Assert.AreEqual(EntityState.Added, GetEntityState(entity));
            }
        }
    }
}
