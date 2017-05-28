#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Cache.HttpRuntime.Test
{
    public partial class CacheTest
    {
        [TestClass]
        public class CacheExistsAsyncTest : CacheTestFixture
        {
            [TestMethod, IntegrationTest]
            public async Task ExistsAsync_Returns_False_When_No_Entry_Exists_With_Specified_Key()
            {
                Assert.IsFalse(await _sut.ExistsAsync(ObjectMother.Create<string>()));
            }

            [TestMethod, IntegrationTest]
            public async Task ExistsAsync_Returns_True_When_Entry_Exists_With_Specified_Key()
            {
                var Key = ObjectMother.Create<string>();

                AddEntry(Key, ObjectMother.Create<string>());

                Assert.IsTrue(await _sut.ExistsAsync(Key));
            }
        }
    }
}
