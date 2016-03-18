#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.Persistance;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.Persistance.Mongo.Test
{
    public partial class BaseRepositoryTest
    {
        [TestClass]
        public class BaseRepositoryGetTest :
            BaseRepositoryFixture
        {
            [TestMethod, IntegrationTest]
            [ExpectedException(typeof(PersistenceException))]
            public void Get_Throws_PersistenceException_When_Entity_Cannot_Be_Found()
            {
                _sut.Get(NonExistingId);
            }

            [TestMethod, IntegrationTest]
            public void Get_Retrieves_Null_When_No_Entity_In_Database_Fulfills_Predicate()
            {
                Assert.IsNull(_sut.Get(e => e.Value == "0"));
            }

            [TestMethod, IntegrationTest]
            public void Get_Retrieves_First_Entity_That_Fulfills_Predicate()
            {
                var entity = _sut.Get(e => e.Value != "0");

                Assert.IsNotNull(entity);
                Assert.IsTrue(entity.Value != "0");
            }
        }
    }
}
