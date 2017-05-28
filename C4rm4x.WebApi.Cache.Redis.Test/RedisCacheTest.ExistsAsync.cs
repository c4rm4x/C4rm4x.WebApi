#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Cache.Redis.Test
{
    public partial class RedisCacheTest
    {
        [TestClass]
        public class RedisCacheExistsAsyncTest : RedisCacheTestFixture
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

                AddEntry(Key, ObjectMother.Create<TestClass>());

                Assert.IsTrue(await _sut.ExistsAsync(Key));
            }
        }
    }
}
