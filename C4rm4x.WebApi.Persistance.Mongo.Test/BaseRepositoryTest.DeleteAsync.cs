#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Persistance.Mongo.Test
{
    public partial class BaseRepositoryTest
    {
        [TestClass]
        public class BaseRepositoryDeleteAsyncTest :
            BaseRepositoryFixture
        {
            [TestMethod, IntegrationTest]
            public async Task DeleteAsync_Removes_Entity_From_Context()
            {
                const string Value1 = "1";

                var entity = GetEntityByValue(Value1);

                await _sut.DeleteAsync(entity);

                Assert.IsNull(GetEntityByValue(Value1));
            }
        }
    }
}
