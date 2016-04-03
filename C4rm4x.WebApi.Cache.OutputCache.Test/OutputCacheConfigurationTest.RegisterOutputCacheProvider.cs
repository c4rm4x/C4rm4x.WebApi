#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Cache.OutputCache.Test
{
    public partial class OutputCacheConfigurationTest
    {
        [TestClass]
        public class OutputCacheConfigurationRegisterOutputCacheProviderTest :
            OutputCacheConfigurationFixture
        {
            [TestMethod, UnitTest]
            public void RegisterOutputCacheProvider_Register_The_ICache_Factory_Within_HttpConfiguration_Properties()
            {
                var config = new HttpConfiguration();

                CreateSubjectUnderTest(config)
                    .RegisterOutputCacheProvider(() => Cache);

                object value;

                Assert.IsTrue(config.Properties.TryGetValue(typeof(ICache), out value));

                var valueAsFunc = value as Func<ICache>;
                Assert.IsNotNull(valueAsFunc);
                Assert.AreSame(Cache, valueAsFunc());
            }
        }
    }
}
