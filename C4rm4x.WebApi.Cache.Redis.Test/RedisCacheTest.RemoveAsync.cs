#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.Cache.Redis.Test
{
    public partial class RedisCacheTest
    {
        [TestClass]
        public class RedisCacheRemoveAsyncTest : RedisCacheTestFixture
        {
            [TestMethod, IntegrationTest]
            public void RemoveAsync_Deletes_Entry_With_Specified_Key()
            {
                var Key = ObjectMother.Create<string>();

                AddEntry(Key, ObjectMother.Create<TestClass>());

                _sut.RemoveAsync(Key).Wait(); // For testing

                Assert.IsNull(GetItem(Key));
            }
        }
    }
}
