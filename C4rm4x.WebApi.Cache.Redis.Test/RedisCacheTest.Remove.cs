#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.Cache.Redis.Test
{
    public partial class RedisCacheTest
    {
        [TestClass]
        public class RedisCacheRemoveTest : RedisCacheTestFixture
        {
            [TestMethod, IntegrationTest]
            public void Remove_Deletes_Entry_With_Specified_Key()
            {
                var Key = ObjectMother.Create<string>();

                AddEntry(Key, ObjectMother.Create<TestClass>());

                _sut.Remove(Key);

                Assert.IsNull(GetItem(Key));
            }
        }
    }
}
