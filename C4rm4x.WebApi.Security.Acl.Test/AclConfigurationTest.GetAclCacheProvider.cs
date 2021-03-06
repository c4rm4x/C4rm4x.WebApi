﻿#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Net.Http;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Security.Acl.Test
{
    public partial class AclConfigurationTest
    {
        [TestClass]
        public class AclConfigurationGetAclCacheProviderTest :
            AclConfigurationFixture
        {
            [TestMethod, UnitTest]
            public void GetAclCacheProvider_Returns_Instance_From_Factory_When_HttpConfiguration_Properties_Contains_An_Entry_Of_Type_ICache()
            {
                var config = new HttpConfiguration();

                RegisterProvider(config, () => Cache);

                Assert.AreSame(
                    Cache,
                    CreateSubjectUnderTest(config)
                        .GetAclCacheProvider(It.IsAny<HttpRequestMessage>()));
            }

            [TestMethod, UnitTest]
            public void GetAclCacheProvider_Returns_Instance_From_Request_Dependency_Scope_When_HttpConfiguration_Properties_Does_Not_Contain_Any_Entry_Of_Type_ICache()
            {
                var OtherCache = Mock.Of<ICache>();

                var sut = CreateSubjectUnderTest(new HttpConfiguration());

                sut.SetResolverFactory((requet, type) => OtherCache);

                Assert.AreSame(
                    OtherCache,
                    sut.GetAclCacheProvider(new HttpRequestMessage()));
            }

            private static void RegisterProvider(
                HttpConfiguration config,
                Func<ICache> provider)
            {
                config.Properties.GetOrAdd(typeof(ICache), obj => provider);
            }
        }
    }
}
