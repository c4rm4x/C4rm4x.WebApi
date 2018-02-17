#region Using

using Autofac.Extras.Multitenant;
using C4rm4x.Tools.HttpUtilities;
using C4rm4x.Tools.TestUtilities;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Autofac.TenantIdentificationStrategies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Web;
using RegularExpressionsMatch = System.Text.RegularExpressions.Match;

#endregion

namespace C4rm4x.WebApi.Framework.Autofac.Test.TenantIdentificationStrategies
{
    public partial class UrlRequestTenantIdentificationStrategyTest
    {
        [TestClass]
        public class UrlRequestTenantIdentificationStrategyTryIdentifyTenantTest
        {
            private const string TenantId = "TenantId";
            private const string Url = "http://localhost/api/method";

            #region Helper classes

            private class TestUrlRequestTenantIdentificationStrategy :
                UrlRequestTenantIdentificationStrategy
            {
                public TestUrlRequestTenantIdentificationStrategy()
                    : base(new Uri(Url).AbsoluteUri)
                {
                }

                protected override object GetTenantId(RegularExpressionsMatch urlMatch)
                {
                    return urlMatch.Success
                        ? TenantId
                        : null;
                }
            }

            #endregion

            [TestMethod, UnitTest]
            public void TryIdentifyTenant_Returns_False_When_HttpContext_Current_Is_Null()
            {
                object tenantId;

                SetupHttpContext(isContextNull: true);

                Assert.IsFalse(TryIdentifyTenant(out tenantId));
                Assert.IsNull(tenantId);
            }

            [TestMethod, UnitTest]
            public void TryIdentifyTenant_Returns_False_When_HttpContext_Current_Request_Is_Null()
            {
                object tenantId;

                SetupHttpContext(isRequestNull: true);

                Assert.IsFalse(TryIdentifyTenant(out tenantId));
                Assert.IsNull(tenantId);
            }

            [TestMethod, UnitTest]
            public void TryIdentifyTenant_Returns_False_When_HttpContext_Current_Request_Url_Is_Null()
            {
                object tenantId;

                SetupHttpContext(url: null);

                Assert.IsFalse(TryIdentifyTenant(out tenantId));
                Assert.IsNull(tenantId);
            }

            [TestMethod, UnitTest]
            public void TryIdentifyTenant_Returns_False_When_HttpContext_Current_Request_Url_Is_Present_But_Does_Not_Match_The_Pattern()
            {
                object tenantId;

                SetupHttpContext(url: "http://localhost/otherapi/method");

                Assert.IsFalse(TryIdentifyTenant(out tenantId));
                Assert.IsNull(tenantId);
            }

            [TestMethod, UnitTest]
            public void TryIdentifyTenant_Returns_True_When_HttpContext_Current_Request_Url_Is_Present_And_Matches_The_Pattern()
            {
                object tenantId;

                SetupHttpContext(url: Url);

                Assert.IsTrue(TryIdentifyTenant(out tenantId));
            }

            [TestMethod, UnitTest]
            public void TryIdentifyTenant_Sets_TenantId_As_TenantId_When_HttpContext_Current_Request_Url_Is_Present_And_Matches_The_Pattern()
            {
                object tenantId;

                SetupHttpContext(url: Url);

                TryIdentifyTenant(out tenantId);

                Assert.AreEqual(TenantId, tenantId);
            }

            private bool TryIdentifyTenant(out object tenantId)
            {
                return CreateSubjectUnderTest()
                    .TryIdentifyTenant(out tenantId);
            }

            private static ITenantIdentificationStrategy CreateSubjectUnderTest()
            {
                return new TestUrlRequestTenantIdentificationStrategy();
            }

            private static void SetupHttpContext(
                bool isContextNull = false,
                bool isRequestNull = false,
                string url = "")
            {
                HttpContextBase currentContext = null;

                if (!isContextNull)
                {
                    var context = Mock.Of<HttpContextBase>();

                    SetupHttpRequest(context, isRequestNull, url);

                    currentContext = context;
                }

                HttpContextFactory.SetCurrentContext(currentContext);
            }

            private static void SetupHttpRequest(
                HttpContextBase context,
                bool isRequestNull,
                string url)
            {
                if (!isRequestNull)
                {
                    var request = Mock.Of<HttpRequestBase>();

                    Mock.Get(context)
                        .SetupGet(c => c.Request)
                        .Returns(request);

                    Mock.Get(request)
                        .SetupGet(r => r.Url)
                        .Returns(url.IsNullOrEmpty() ? null : new Uri(url));
                }
            }
        }
    }
}
