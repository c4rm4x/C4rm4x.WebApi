#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Caching;
using Caching = System.Web.Caching.Cache;

#endregion

namespace C4rm4x.WebApi.Cache.HttpRuntime.Test
{
    public partial class CacheTest
    {
        #region Helper classes

        private class TestClass { }

        #endregion

        protected static System.Web.Caching.Cache HttpCache =>
            System.Web.HttpRuntime.Cache;

        [TestClass]
        public abstract class CacheTestFixture :
            IntegrationFixture<Cache>
        {
            [TestCleanup]
            public override void Cleanup()
            {
                FlushCache();

                base.Cleanup();
            }

            private static void FlushCache()
            {
                var enumerator = HttpCache.GetEnumerator();

                while (enumerator.MoveNext())
                    HttpCache.Remove(enumerator.Key.ToString());
            }

            protected static void AddEntry<TValue>(
                string key,
                TValue value)
            {
                HttpCache.Add(key, value, null,
                    DateTime.Now.AddSeconds(10),
                    Caching.NoSlidingExpiration,
                    CacheItemPriority.Normal, null);
            }
        }
    }
}
