#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Persistance.Document.Test.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.Persistance.Document.Test
{
    public partial class DatabaseInitialiserTest
    {
        [TestClass]
        public class DatabaseInitialiserGetOrCreateDatabaseTest :
            BasePersistenceFixture<DatabaseInitialiser>
        {
            [TestMethod, IntegrationTest]
            public void GetOrCreateDatabase_Returns_An_Instance_Of_Database()
            {
                var database = _sut.GetOrCreateDatabase(TestDatabase);

                Assert.IsNotNull(database);
                Assert.AreEqual(TestDatabase, database.Id);
            }
        }
    }
}
