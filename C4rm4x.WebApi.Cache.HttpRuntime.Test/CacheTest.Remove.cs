#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.Cache.HttpRuntime.Test
{
    public partial class CacheTest
    {
        [TestClass]
        public class CacheRemoveTest : CacheTestFixture
        {
            [TestMethod, IntegrationTest]
            public void Remove_Deletes_Entry_With_Specified_Key()
            {
                var Key = ObjectMother.Create<string>();

                AddEntry(Key, ObjectMother.Create<string>());

                _sut.Remove(Key);

                Assert.IsNull(HttpCache[Key]);
            }
        }
    }
}
