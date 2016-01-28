#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

#endregion

namespace C4rm4x.WebApi.Cache.HttpRuntime.Test
{
    public partial class CacheTest
    {
        [TestClass]
        public class CacheStoreTest : CacheTestFixture
        {
            [TestMethod, UnitTest]
            public void Store_Caches_An_Entry_With_Specified_Key()
            {
                var Key = ObjectMother.Create<string>();
                var Value = ObjectMother.Create<TestClass>();

                _sut.Store(Key, Value);

                var result = HttpCache[Key];

                Assert.IsNotNull(result);
                Assert.AreSame(Value, result);
            }

            [TestMethod, IntegrationTest]
            public void Store_Caches_An_Entry_With_Specified_Key_For_ExpirationTime_Minutes()
            {
                const int Milliseconds = 1000;

                var Key = ObjectMother.Create<string>();
                var Value = ObjectMother.Create<TestClass>();
                var expirationTime = GetRand(5);

                _sut.Store(Key, Value, expirationTime);

                Thread.Sleep((expirationTime + 1) * Milliseconds);

                Assert.IsNull(HttpCache[Key]);
            }

            private static int GetRand(int max)
            {
                return new Random().Next(1, max);
            }
        }
    }
}
