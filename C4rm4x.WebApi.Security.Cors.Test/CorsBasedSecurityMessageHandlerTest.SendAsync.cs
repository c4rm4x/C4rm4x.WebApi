#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.Tools.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Security.Cors.Test
{
    public partial class CorsBasedSecurityMessageHandlerTest
    {
        [TestClass]
        public class CorsBasedSecurityMessageHandlerSendAsyncTest
        {
            private const string MethodTypeGet = "GET";
            private const string MethodTypePost = "POST";
            private const string MethodTypeOptions = "OPTIONS";
            private const string MethodTypePut = "PUT";
            private const string MethodTypeDelete = "DELETE";
            private const string MethodTypeHead = "HEAD";
            private const string MethodTypeTrace = "TRACE";

            [TestMethod, UnitTest]
            public void SendAsync_Returns_InnerHandler_Result_When_Origin_Header_Is_Not_Present()
            {
                var Response = new HttpResponseMessage();

                Assert.AreSame(
                    Response,
                    SendAsync(origin: string.Empty, response: Response).Result);
            }

            [TestMethod, UnitTest]
            public void SendAsync_Returns_Forbiden_Response_When_Origin_Header_Is_Present_But_CORS_Policy_Is_Not_Met()
            {
                Assert.AreEqual(
                    HttpStatusCode.Forbidden,
                    SendAsync(isCorsValid: false).Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void SendAsync_Returns_Ok_Response_When_Access_Control_Request_Method_Is_Present_In_A_Valid_CORS_Preflight_Request()
            {
                Assert.AreEqual(
                    HttpStatusCode.OK,
                    SendAsync(methodType: MethodTypeOptions, accessControlRequestMethod: "anyRequest")
                        .Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void SendAsync_Returns_A_Response_With_CORS_Headers_When_Access_Control_Request_Method_Is_Present_In_A_Valid_CORS_Preflight_Request()
            {
                var responseHeader = new KeyValuePair<string, string>(
                    ObjectMother.Create<string>(), ObjectMother.Create<string>());

                var response = SendAsync(
                    handler: CreateSubjectUnderTest(corsEngine: GetCorsEngine(true, responseHeader)),
                    methodType: MethodTypeOptions, accessControlRequestMethod: "anyRequest").Result;

                Assert.IsTrue(response.Headers.Any(h => h.Key == responseHeader.Key && h.Value.Contains(responseHeader.Value)));
            }

            [TestMethod, UnitTest]
            public void SendAsync_Returns_InnerHandler_Result_When_A_Valid_CORS_Not_Preflight_Request_Arrives()
            {
                var Response = new HttpResponseMessage();

                Assert.AreSame(
                    Response,
                    SendAsync(methodType: MethodTypePost, response: Response).Result);
            }

            [TestMethod, UnitTest]
            public void SendAsync_Returns_InnerHandler_Result_With_CORS_Headers_When_A_Valid_CORS_Not_Preflight_Request_Arrives()
            {
                var Response = new HttpResponseMessage();
                var responseHeader = new KeyValuePair<string, string>(
                    ObjectMother.Create<string>(), ObjectMother.Create<string>());

                var response = SendAsync(
                    handler: CreateSubjectUnderTest(response: Response, corsEngine: GetCorsEngine(true, responseHeader)),
                    methodType: MethodTypePost).Result;

                Assert.IsTrue(response.Headers.Any(h => h.Key == responseHeader.Key && h.Value.Contains(responseHeader.Value)));
            }

            private static Task<HttpResponseMessage> SendAsync(
                string origin = null,
                HttpResponseMessage response = null)
            {
                return new HttpMessageInvoker(CreateSubjectUnderTest(response))
                    .SendAsync(
                        GetHttpRequestMessage(origin ?? ObjectMother.Create<string>()),
                        It.IsAny<CancellationToken>());
            }

            private static CorsBasedSecurityMessageHandler CreateSubjectUnderTest(
                HttpResponseMessage response = null,
                CorsEngine corsEngine = null,
                CorsOptions options = null)
            {
                var sut = new CorsBasedSecurityMessageHandler(options ?? CorsOptions.AllowAll());

                sut.InnerHandler = new TestHandler(response);
                sut.SetCorsEngineFactory(() => corsEngine ?? new CorsEngine());

                return sut;
            }

            private static HttpRequestMessage GetHttpRequestMessage(
                string origin,
                string methodType = MethodTypeGet,
                string accessControlRequestMethod = "anyHeader")
            {
                var requestMessage = new HttpRequestMessage();

                if (!origin.IsNullOrEmpty())
                    requestMessage.Headers
                        .Add(CorsConstants.Origin, origin);

                requestMessage.Method = GetHttpMethod(methodType);

                if (!accessControlRequestMethod.IsNullOrEmpty())
                    requestMessage.Headers
                        .Add(CorsConstants.AccessControlRequestMethod, accessControlRequestMethod);

                return requestMessage;
            }

            private static Task<HttpResponseMessage> SendAsync(
                bool isCorsValid = true,
                string methodType = MethodTypeGet,
                string accessControlRequestMethod = "anyHeader",
                HttpResponseMessage response = null)
            {
                return SendAsync(CreateSubjectUnderTest(isCorsValid, response), methodType, accessControlRequestMethod);
            }

            private static Task<HttpResponseMessage> SendAsync(
                CorsBasedSecurityMessageHandler handler,
                string methodType = MethodTypeGet,
                string accessControlRequestMethod = "anyHeader")
            {
                return new HttpMessageInvoker(handler)
                    .SendAsync(
                        GetHttpRequestMessage(ObjectMother.Create<string>(), methodType, accessControlRequestMethod),
                        It.IsAny<CancellationToken>());
            }

            private static CorsBasedSecurityMessageHandler CreateSubjectUnderTest(
                bool isCorsValid,
                HttpResponseMessage response = null)
            {
                return CreateSubjectUnderTest(response, GetCorsEngine(isCorsValid));
            }

            private static CorsEngine GetCorsEngine(
                bool isCorsValid,
                params KeyValuePair<string, string>[] responseHeaders)
            {
                var corsEngine = Mock.Of<CorsEngine>();

                Mock.Get(corsEngine)
                    .Setup(e => e.EvaluatePolicy(It.IsAny<CorsRequestContext>(), It.IsAny<CorsPolicy>()))
                    .Returns(GetResult(isCorsValid, responseHeaders));

                return corsEngine;
            }

            private static CorsResult GetResult(
                bool isCorsValid,
                params KeyValuePair<string, string>[] responseHeaders)
            {
                if (!isCorsValid)
                    return null;

                var result = Mock.Of<CorsResult>();

                Mock.Get(result)
                    .Setup(r => r.ToResponseHeaders())
                    .Returns(GetResponseHeaders(responseHeaders));

                return result;
            }

            private static IDictionary<string, string> GetResponseHeaders(
                KeyValuePair<string, string>[] responseHeaders)
            {
                var result = new Dictionary<string, string>();

                foreach (var responseHeader in responseHeaders)
                    result.Add(responseHeader.Key, responseHeader.Value);

                return result;
            }

            private static HttpMethod GetHttpMethod(string methodType)
            {
                if (methodType.Equals(MethodTypeDelete))
                    return HttpMethod.Delete;

                if (methodType.Equals(MethodTypeGet))
                    return HttpMethod.Get;

                if (methodType.Equals(MethodTypeHead))
                    return HttpMethod.Head;

                if (methodType.Equals(MethodTypeOptions))
                    return HttpMethod.Options;

                if (methodType.Equals(MethodTypePost))
                    return HttpMethod.Post;

                if (methodType.Equals(MethodTypePut))
                    return HttpMethod.Put;

                if (methodType.Equals(MethodTypeTrace))
                    return HttpMethod.Trace;

                throw new ArgumentException("Method type not supported", nameof(methodType));
            }

            #region Helper classes

            class TestHandler : DelegatingHandler
            {
                public HttpResponseMessage ResponseMessage { get; set; }

                public TestHandler(HttpResponseMessage responseMessage)
                {
                    ResponseMessage = responseMessage;
                }

                protected override Task<HttpResponseMessage> SendAsync(
                    HttpRequestMessage request,
                    CancellationToken cancellationToken)
                {
                    return Task.FromResult(ResponseMessage);
                }
            }


            #endregion
        }
    }
}
