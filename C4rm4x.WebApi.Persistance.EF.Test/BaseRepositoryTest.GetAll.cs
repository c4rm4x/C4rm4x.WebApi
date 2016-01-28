#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Persistance.EF.Test
{
    public partial class BaseRepositoryTest
    {
        [TestClass]
        public class BaseRepositoryGetAllTest :
            BaseRepositoryFixture
        {
            [TestMethod, IntegrationTest]
            public void GetAll_Retrieves_All_Entities_In_Database()
            {
                var entities = _sut.GetAll();

                Assert.IsNotNull(entities);
                Assert.IsTrue(entities.Any());
                Assert.AreEqual(10, entities.Count);

                foreach (var entity in entities)
                {
                    Assert.IsNotNull(entity);
                    Assert.AreEqual(entity.Id.ToString(), entity.Value);
                }
            }

            [TestMethod, IntegrationTest]
            public void GetAll_Retrieves_Empty_Collection_When_No_Entity_In_Database_Fulfills_Predicate()
            {
                var entities = _sut.GetAll(e => e.Id < 0);

                Assert.IsNotNull(entities);
                Assert.IsFalse(entities.Any());
            }

            [TestMethod, IntegrationTest]
            public void GetAll_Retrieves_A_Collection_With_All_Entities_In_Database_That_Fulfill_Predicate()
            {
                var entities = _sut.GetAll(e => e.Id > 0);

                Assert.IsNotNull(entities);
                Assert.IsTrue(entities.Any());

                foreach (var entity in entities)
                {
                    Assert.IsNotNull(entity);
                    Assert.IsTrue(entity.Id > 0);
                    Assert.AreEqual(entity.Id.ToString(), entity.Value);
                }
            }
        }
    }
}
