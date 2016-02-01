#region Using

using Autofac.Extras.Multitenant;
using C4rm4x.Tools.HttpUtilities;
using C4rm4x.Tools.TestUtilities;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Autofac.Extensions.TenantIdentificationStrategies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Specialized;
using System.Web;

#endregion

namespace C4rm4x.WebApi.Framework.Autofac.Extensions.Test.TenantIdentificationStrategies
{
    public partial class HeaderRequestTenantIdentificationStrategyTest
    {
        [TestClass]
        public class HeaderRequestTenantIdentificationStrategyTryIdentifyTenantTest
        {
            private const string Header = "Header";
            private const string TenantId = "TenantId";

            #region Helper classes

            private class TestHeaderRequestTenantIdentificationStrategy :
                HeaderRequestTenantIdentificationStrategy
            {
                public TestHeaderRequestTenantIdentificationStrategy()
                    : base(Header)
                { }

                protected override object GetTenantId(string headerValue)
                {
                    return TenantId;
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
            public void TryIdentifyTenant_Returns_False_When_HttpContext_Current_Request_Does_Not_Have_The_Header()
            {
                object tenantId;

                SetupHttpContext(header: null);

                Assert.IsFalse(TryIdentifyTenant(out tenantId));
                Assert.IsNull(tenantId);
            }

            [TestMethod, UnitTest]
            public void TryIdentifyTenant_Returns_True_When_HttpContext_Current_Request_Does_Have_The_Header()
            {
                object tenantId;

                SetupHttpContext(header: Header);

                Assert.IsTrue(TryIdentifyTenant(out tenantId));
            }

            [TestMethod, UnitTest]
            public void TryIdentifyTenant_Sets_TenantId_As_TenantId_When_HttpContext_Current_Request_Does_Have_The_Header()
            {
                object tenantId;

                SetupHttpContext(header: Header);

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
                return new TestHeaderRequestTenantIdentificationStrategy();
            }

            private static void SetupHttpContext(
                bool isContextNull = false,
                bool isRequestNull = false,
                string header = "")
            {
                HttpContextBase currentContext = null;

                if (!isContextNull)
                {
                    var context = Mock.Of<HttpContextBase>();

                    SetupHttpRequest(context, isRequestNull, header);

                    currentContext = context;
                }

                HttpContextFactory.SetCurrentContext(currentContext);
            }

            private static void SetupHttpRequest(
                HttpContextBase context,
                bool isRequestNull,
                string header)
            {
                if (!isRequestNull)
                {
                    var request = Mock.Of<HttpRequestBase>();

                    Mock.Get(context)
                        .SetupGet(c => c.Request)
                        .Returns(request);

                    if (!header.IsNullOrEmpty())
                        Mock.Get(request)
                            .SetupGet(r => r.Headers)
                            .Returns(GetHeaders(header));
                }
            }

            private static NameValueCollection GetHeaders(string header)
            {
                var headers = new NameValueCollection();

                headers.Add(header, ObjectMother.Create<string>());

                return headers;
            }
        }
    }
}
