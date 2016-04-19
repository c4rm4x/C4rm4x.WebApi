#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Security.Acl.Test
{
    public partial class AclConfigurationTest
    {
        [TestClass]
        public class AclConfigurationRegisterAclCacheProviderTest :
            AclConfigurationFixture
        {
            [TestMethod, UnitTest]
            public void RegisterAclCacheProvider_Register_The_ICache_Factory_Within_HttpConfiguration_Properties()
            {
                var config = new HttpConfiguration();

                CreateSubjectUnderTest(config)
                    .RegisterAclCacheProvider(() => Cache);

                object value;

                Assert.IsTrue(config.Properties.TryGetValue(typeof(ICache), out value));

                var valueAsFunc = value as Func<ICache>;
                Assert.IsNotNull(valueAsFunc);
                Assert.AreSame(Cache, valueAsFunc());
            }
        }
    }
}
