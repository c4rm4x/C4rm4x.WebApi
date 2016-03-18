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
        public class BaseRepositoryDeleteTest :
            BaseRepositoryFixture
        {
            [TestMethod, IntegrationTest]
            [ExpectedException(typeof(PersistenceException))]
            public void Delete_Throws_PersistenceException_When_Entity_Cannot_Be_Found()
            {
                _sut.Delete(NonExistingId);
            }

            [TestMethod, IntegrationTest]
            public void Delete_Removes_Entity_From_Context()
            {
                const string Value1 = "1";

                var entity = GetEntityByValue(Value1);

                _sut.Delete(entity);

                Assert.IsNull(GetEntityByValue(Value1));
            }
        }
    }
}
