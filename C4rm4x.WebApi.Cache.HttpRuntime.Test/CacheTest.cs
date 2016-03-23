#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        }
    }
}
