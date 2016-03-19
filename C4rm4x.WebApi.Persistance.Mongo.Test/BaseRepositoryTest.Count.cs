#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.Persistance.Mongo.Test
{
    public partial class BaseRepositoryTest
    {
        [TestClass]
        public class BaseRepositoryCountTest :
            BaseRepositoryFixture
        {
            [TestMethod, IntegrationTest]
            public void Count_Returns_The_Number_Of_All_Entities_In_Database()
            {
                Assert.AreEqual(10, _sut.Count());
            }

            [TestMethod, IntegrationTest]
            public void Count_Returns_0_When_No_Entity_In_Database_Fulfills_Predicate()
            {
                Assert.AreEqual(0, _sut.Count(e => e.Value == "0"));
            }

            [TestMethod, IntegrationTest]
            public void Count_Returns_The_Total_Number_Of_Entities_In_Database_That_Fulfill_Predicate()
            {
                Assert.AreEqual(10, _sut.Count(e => e.Value != "0"));
            }
        }
    }
}
