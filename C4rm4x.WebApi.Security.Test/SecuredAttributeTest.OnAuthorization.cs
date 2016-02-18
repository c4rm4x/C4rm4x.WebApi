#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.Tools.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Http.Controllers;
using System;
using System.Net.Http;

#endregion

namespace C4rm4x.WebApi.Security.Test
{
    public partial class SecuredAttributeTest
    {
        [TestClass]
        public class SecuredAttributeOnAuthorizationTest
        {
            [TestMethod, UnitTest]
            public void OnAuthorization_Returns_Empty_Response_When_Method_Is_Decorated_With_AllowAnonymousAttribute_Even_Principal_Is_Not_Authenticated()
            {
                var actionContext = GetActionContext(allowsAnonymous: true, isAuthenticated: false);

                CreateSubjectUnderTest()
                    .OnAuthorization(actionContext);

                Assert.IsNull(actionContext.Response);
            }

            [TestMethod, UnitTest]
            public void OnAuthorization_Returns_HttpStatusCode_Unauthorized_When_Principal_Is_Not_Authenticated()
            {
                var actionContext = GetActionContext(isAuthenticated: false);

                CreateSubjectUnderTest()
                    .OnAuthorization(actionContext);

                Assert.AreEqual(HttpStatusCode.Unauthorized, actionContext.Response.StatusCode);
            }

            [TestMethod, UnitTest]
            public void OnAuthorization_Returns_Empty_Response_When_Principal_Is_Authenticated()
            {
                var actionContext = GetActionContext(isAuthenticated: true);

                CreateSubjectUnderTest()
                    .OnAuthorization(actionContext);

                Assert.IsNull(actionContext.Response);
            }

            [TestMethod, UnitTest]
            public void OnAuthorization_Returns_HttpStatusCode_Forbidden_When_Principal_Is_Not_In_Specified_Role()
            {
                var actionContext = GetActionContext(role: "Role");

                CreateSubjectUnderTest(role: "OtherRole")
                    .OnAuthorization(actionContext);

                Assert.AreEqual(HttpStatusCode.Forbidden, actionContext.Response.StatusCode);
            }

            [TestMethod, UnitTest]
            public void OnAuthorization_Returns_Empty_Response_When_Principal_Is_In_Specified_Role()
            {
                var actionContext = GetActionContext(role: "Role");

                CreateSubjectUnderTest("Role")
                    .OnAuthorization(actionContext);

                Assert.IsNull(actionContext.Response);
            }

            [TestMethod, UnitTest]
            public void OnAuthorization_Returns_HttpStatusCode_Forbidden_When_Principal_Does_Not_Have_Specified_Claim()
            {
                var actionContext = GetActionContext(claim: new Claim("ClaimType", "ClaimValue"));

                CreateSubjectUnderTest(claim: new Claim("OtherClaimType", "OtherClaimValue"))
                    .OnAuthorization(actionContext);

                Assert.AreEqual(HttpStatusCode.Forbidden, actionContext.Response.StatusCode);
            }

            [TestMethod, UnitTest]
            public void OnAuthorization_Returns_Empty_Response_When_Principal_Has_Specified_Claim()
            {
                var actionContext = GetActionContext(claim: new Claim("ClaimType", "ClaimValue"));

                CreateSubjectUnderTest(claim: new Claim("ClaimType", "ClaimValue"))
                    .OnAuthorization(actionContext);

                Assert.IsNull(actionContext.Response);
            }

            private static SecuredAttribute CreateSubjectUnderTest(
                string role = "",
                Claim claim = null)
            {
                var sut = new SecuredAttribute();

                if (!role.IsNullOrEmpty())
                    sut.Role = role;

                if (claim.IsNotNull())
                    sut.Claim = claim;

                return sut;
            }

            private static HttpActionContext GetActionContext(
                bool allowsAnonymous = false,
                bool isAuthenticated = true,
                string role = "",
                Claim claim = null)
            {
                return new HttpActionContext(
                    GetControllerContext(isAuthenticated, role, claim),
                    GetActionDescriptor(allowsAnonymous));
            }

            private static HttpActionDescriptor GetActionDescriptor(bool allowsAnonymous)
            {
                var actionDescriptor = Mock.Of<HttpActionDescriptor>();

                Mock.Get(actionDescriptor)
                    .Setup(a => a.GetCustomAttributes<AllowAnonymousAttribute>())
                    .Returns(new Collection<AllowAnonymousAttribute>(
                        GetAllowAnonymousAttribute(allowsAnonymous).ToList()));

                return actionDescriptor;
            }

            private static IEnumerable<AllowAnonymousAttribute> GetAllowAnonymousAttribute(bool allowsAnonymous)
            {
                if (allowsAnonymous)
                    yield return new AllowAnonymousAttribute();

                yield break;
            }

            private static HttpControllerContext GetControllerContext(
                bool isAuthenticated, 
                string role, 
                Claim claim)
            {
                return new HttpControllerContext(
                    GetRequestContext(isAuthenticated, role, claim),
                    Mock.Of<HttpRequestMessage>(), 
                    Mock.Of<HttpControllerDescriptor>(), 
                    Mock.Of<IHttpController>());
            }

            private static HttpRequestContext GetRequestContext(
                bool isAuthenticated, 
                string role, 
                Claim claim)
            {
                var requestContext = Mock.Of<HttpRequestContext>();

                Mock.Get(requestContext)
                    .SetupGet(r => r.Principal)
                    .Returns(GetPrincipal(isAuthenticated, role, claim));

                return requestContext;
            }

            private static IPrincipal GetPrincipal(
                bool isAuthenticated, 
                string role, 
                Claim claim)
            {
                var principal = Mock.Of<ClaimsPrincipal>();

                Mock.Get(principal)
                    .SetupGet(p => p.Identity)
                    .Returns(GetIdentity(isAuthenticated));

                IsInRole(principal, role);
                HasClaim(principal, claim);

                return principal;
            }

            private static IIdentity GetIdentity(bool isAuthenticated)
            {
                var identity = Mock.Of<IIdentity>();

                Mock.Get(identity)
                    .SetupGet(i => i.IsAuthenticated)
                    .Returns(isAuthenticated);

                return identity;
            }

            private static void IsInRole(
                IPrincipal principal,
                string role)
            {
                if (role.IsNullOrEmpty()) return;

                Mock.Get(principal)
                    .Setup(p => p.IsInRole(role))
                    .Returns(true);
            }

            private static void HasClaim(
                ClaimsPrincipal principal,
                Claim claim)
            {
                if (claim.IsNull()) return;

                Mock.Get(principal)
                    .Setup(p => p.HasClaim(claim.Type, claim.Value))
                    .Returns(true);
            }
        }
    }
}
