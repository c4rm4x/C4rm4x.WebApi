#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.Cache.Redis.Test
{
    public partial class RedisCacheTest
    {
        [TestClass]
        public class RedisCacheExistsTest : RedisCacheTestFixture
        {
            [TestMethod, IntegrationTest]
            public void Exists_Returns_False_When_No_Entry_Exists_With_Specified_Key()
            {
                Assert.IsFalse(_sut.Exists(ObjectMother.Create<string>()));
            }

            [TestMethod, IntegrationTest]
            public void Exists_Returns_True_When_Entry_Exists_With_Specified_Key()
            {
                var Key = ObjectMother.Create<string>();

                AddEntry(Key, ObjectMother.Create<TestClass>());

                Assert.IsTrue(_sut.Exists(Key));
            }
        }
    }
}
