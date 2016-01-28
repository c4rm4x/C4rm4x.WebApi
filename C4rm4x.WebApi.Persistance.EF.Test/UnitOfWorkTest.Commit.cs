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
        public class UnitOfWorkCommitTest : UnitOfWorkFixture
        {
            [TestMethod, IntegrationTest]
            public void Commit_Saves_Context_Into_Database()
            {
                var entity = new TestTable(Value);

                SetEntityState(entity, EntityState.Added);

                Assert.AreEqual(0, entity.Id);

                _sut.Commit();

                Assert.AreNotEqual(0, entity.Id);
            }
        }
    }
}
