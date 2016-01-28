#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.Persistance;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.Persistance.EF.Test
{
    public partial class BaseRepositoryTest
    {
        [TestClass]
        public class BaseRepositoryExecuteNonQueryTest
            : BaseRepositoryFixture
        {
            [TestMethod, IntegrationTest]
            [ExpectedException(typeof(PersistenceException))]
            public void ExecuteNonQuery_Throws_PersistenceException_When_Any_Error_Occurs()
            {
                _sut.ExecuteNonQuery("non_existing_sp");
            }
        }
    }
}
