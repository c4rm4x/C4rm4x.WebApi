#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Cache.Redis.Test
{
    public partial class RedisCacheTest
    {
        [TestClass]
        public class RedisCacheRetrieveAsyncTest : RedisCacheTestFixture
        {
            [TestMethod, IntegrationTest]
            public async Task RetrieveAsync_Returns_Null_When_No_Entry_Exists_With_Specified_Key()
            {
                Assert.IsNull(await _sut.RetrieveAsync(ObjectMother.Create<string>()));
            }

            [TestMethod, IntegrationTest]
            public async Task RetrieveAsync_Returns_Value_When_Entry_Exists_With_Specified_Key()
            {
                var Key = ObjectMother.Create<string>();
                var Value = ObjectMother.Create<string>();

                AddEntry(Key, Value);

                var result = await _sut.RetrieveAsync(Key);

                Assert.IsNotNull(result);
                Assert.AreEqual(Value, result.ToString());
            }

            [TestMethod, IntegrationTest]
            [ExpectedException(typeof(InvalidCastException))]
            public async Task RetrieveAsync_Throws_InvalidCastException_When_Value_Of_Entry_With_Specified_Key_Is_Not_Of_Specified_Type()
            {
                var Key = ObjectMother.Create<string>();
                var Value = ObjectMother.Create<string>();

                AddEntry(Key, Value);

                await _sut.RetrieveAsync<int>(Key);
            }

            [TestMethod, IntegrationTest]
            public async Task RetrieveAsync_Returns_Value_As_Specified_Type_When_Entry_With_Specified_Key_Exists_And_Value_Is_Of_Specified_Type()
            {
                var Key = ObjectMother.Create<string>();
                var Value = ObjectMother.Create<TestClass>();

                AddEntry<TestClass>(Key, Value);

                var result = await _sut.RetrieveAsync<TestClass>(Key);

                Assert.IsNotNull(result);
                Assert.AreEqual(Value, result);
            }

            private static void AddEntry(
                string key,
                string value)
            {
                Cache.StringSet(key, value);
            }
        }
    }
}
