#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.Cache.HttpRuntime.Test
{
    public partial class CacheTest
    {
        [TestClass]
        public class CacheRemoveAsyncTest : CacheTestFixture
        {
            [TestMethod, IntegrationTest]
            public void RemoveAsync_Deletes_Entry_With_Specified_Key()
            {
                var Key = ObjectMother.Create<string>();

                AddEntry(Key, ObjectMother.Create<string>());

                _sut.RemoveAsync(Key).Wait(); // For testing

                Assert.IsNull(HttpCache[Key]);
            }
        }
    }
}
