#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Cache;
using C4rm4x.WebApi.Security.Acl.Subscriptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Hosting;
using System;
using System.Linq;
using System.Web.Http;
using System.Security.Principal;

#endregion

namespace C4rm4x.WebApi.Security.Acl.Test
{
    public partial class AclBasedSecurityMessageHandlerTest
    {
        [TestClass]
        public class AclBasedSecurityMessageHandlerSendAsyncTest
        {
            private const string Subscriber = "Subscriber";
            private const string Secret = "5d41402abc4b2a76b9719d911017c592";
            private const string ValidAuthorizationHeader = "U3Vic2NyaWJlcjpoZWxsbw=="; // Subscriber:hello

            [TestMethod, UnitTest]
            public void SendAsync_Returns_InnerHandler_Result_When_Authorization_Header_Is_Not_Present_But_ForceAuthentication_Is_False()
            {
                var Response = new HttpResponseMessage();

                Assert.AreSame(
                    Response,
                    SendAsync(authorization: string.Empty, forceAuthentication: false, response: Response).Result);
            }

            [TestMethod, UnitTest]
            public void SendAsync_Returns_Unauthorized_Response_When_Authorization_Header_Is_Not_Present_And_ForceAuthentication_Is_True()
            {
                Assert.AreEqual(
                    HttpStatusCode.Unauthorized,
                    SendAsync(authorization: string.Empty, forceAuthentication: true).Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void SendAsync_Returns_InnerHandler_Result_When_Authorization_Header_Value_Is_Not_A_Valid_Header_But_ForceAuthentication_Is_False()
            {
                var Response = new HttpResponseMessage();

                Assert.AreSame(
                    Response,
                    SendAsync(authorization: ObjectMother.Create<string>(), forceAuthentication: false, response: Response).Result);
            }

            [TestMethod, UnitTest]
            public void SendAsync_Returns_Unauthorized_Response_When_Authorization_Header_Value_Is_Not_A_Valid_Header_And_ForceAuthentication_Is_True()
            {
                Assert.AreEqual(
                    HttpStatusCode.Unauthorized,
                    SendAsync(authorization: ObjectMother.Create<string>(), forceAuthentication: true).Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void SendAsync_Uses_Retrieve_From_ICache_To_Retrieve_All_Subscriber_When_Authorization_Header_Is_A_Valid_Header()
            {
                var cache = Mock.Of<ICache>();

                var result = SendAsync(
                    authorization: ValidAuthorizationHeader, cache: cache)
                    .Result;

                Mock.Get(cache)
                    .Verify(c => c.RetrieveAsync<IEnumerable<Subscriber>>(AclConfiguration.SubscribersCacheKey),
                    Times.Once());
            }

            [TestMethod, UnitTest]
            public void SendAsync_Does_Not_Use_GetAll_From_ISubscriberRepository_To_Retrieve_All_Subscriber_When_Authorization_Header_Is_A_Valid_Header_And_Cache_Is_Not_Empty()
            {
                var cache = Mock.Of<ICache>();
                var repository = Mock.Of<ISubscriberRepository>();

                Mock.Get(cache)
                    .Setup(c => c.RetrieveAsync<IEnumerable<Subscriber>>(AclConfiguration.SubscribersCacheKey))
                    .Returns(Task.FromResult(GetSubscribers().AsEnumerable()));

                var result = SendAsync(
                    authorization: ValidAuthorizationHeader,
                    cache: cache,
                    subscriberRepository: repository)
                    .Result;

                Mock.Get(repository)
                    .Verify(c => c.GetAllAsync(), Times.Never());
            }

            [TestMethod, UnitTest]
            public void SendAsync_Uses_GetAll_From_ISubscriberRepository_To_Retrieve_All_Subscriber_When_Authorization_Header_Is_A_Valid_Header_And_Cache_Is_Empty()
            {
                var repository = Mock.Of<ISubscriberRepository>();

                var result = SendAsync(
                    authorization: ValidAuthorizationHeader,
                    subscriberRepository: repository)
                    .Result;

                Mock.Get(repository)
                    .Verify(c => c.GetAllAsync(), Times.Once());
            }

            [TestMethod, UnitTest]
            public void SendAsync_Uses_Store_From_ICache_To_Store_All_Subscriberes_Returned_By_ISubscriberRepository_For_One_Hour()
            {
                var cache = Mock.Of<ICache>();
                var repository = Mock.Of<ISubscriberRepository>();
                var subscribers = GetSubscribers().AsQueryable();

                Mock.Get(repository)
                    .Setup(r => r.GetAllAsync())
                    .Returns(Task.FromResult(subscribers));

                var result = SendAsync(
                    authorization: ValidAuthorizationHeader,
                    cache: cache,
                    subscriberRepository: repository)
                    .Result;

                Mock.Get(cache)
                    .Verify(c => c.StoreAsync(AclConfiguration.SubscribersCacheKey, subscribers, 3600), 
                    Times.Once());
            }

            [TestMethod, UnitTest]
            public void SendAsync_Returns_Unauthorized_Response_When_Authorization_Header_Is_Present_But_There_Are_No_Subscribers_With_Given_Identificator()
            {
                var cache = Mock.Of<ICache>();

                Mock.Get(cache)
                    .Setup(c => c.RetrieveAsync<IEnumerable<Subscriber>>(AclConfiguration.SubscribersCacheKey))
                    .Returns(Task.FromResult(GetSubscribers().AsEnumerable()));

                Assert.AreEqual(
                    HttpStatusCode.Unauthorized,
                    SendAsync(
                        authorization: ValidAuthorizationHeader,
                        cache: cache).Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void SendAsync_Returns_Unauthorized_Response_When_Authorization_Header_Is_Present_And_There_Is_A_Subscriber_With_Given_Identificator_But_Secret_Is_Incorrect()
            {
                var cache = Mock.Of<ICache>();

                Mock.Get(cache)
                    .Setup(c => c.RetrieveAsync<IEnumerable<Subscriber>>(AclConfiguration.SubscribersCacheKey))
                    .Returns(Task.FromResult(new[]
                    {
                        GetSubscriber(secret: "5a8dd3ad0756a93ded72b823b19dd877") // hello!
                    }
                    .AsEnumerable())); 

                Assert.AreEqual(
                    HttpStatusCode.Unauthorized,
                    SendAsync(
                        authorization: ValidAuthorizationHeader,
                        cache: cache).Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void SendAsync_Returns_InnerHandler_Result_When_Authorization_Header_Is_Present_And_There_Is_A_Subscriber_With_Given_Identificator_And_Secret_Is_Correct()
            {
                var cache = Mock.Of<ICache>();
                var response = new HttpResponseMessage();

                Mock.Get(cache)
                    .Setup(c => c.RetrieveAsync<IEnumerable<Subscriber>>(AclConfiguration.SubscribersCacheKey))
                    .Returns(Task.FromResult(new[]
                    {
                        GetSubscriber(secret: Secret)
                    }
                    .AsEnumerable()));

                Assert.AreSame(
                    response,
                    SendAsync(
                        authorization: ValidAuthorizationHeader,
                        response: response,
                        cache: cache).Result);
            }

            [TestMethod, UnitTest]
            public void SendAsync_Sets_Thread_CurrentPrincipal_When_A_Valid_Authorization_Header_Is_Present_And_Subscriber_With_Given_Identifier_And_Secret_Exists()
            {
                var cache = Mock.Of<ICache>();
                var response = new HttpResponseMessage();

                Mock.Get(cache)
                    .Setup(c => c.RetrieveAsync<IEnumerable<Subscriber>>(AclConfiguration.SubscribersCacheKey))
                    .Returns(Task.FromResult(new[]
                    {
                        GetSubscriber(secret: Secret)
                    }
                    .AsEnumerable()));

                SendAsync(
                    authorization: ValidAuthorizationHeader,
                    cache: cache);

                Assert.IsNotNull(Thread.CurrentPrincipal);
            }

            [TestMethod, UnitTest]
            public void SendAsync_Sets_Request_Context_Principal_When_A_Valid_Authorization_Header_Is_Present_And_Subscriber_With_Given_Identifier_And_Secret_Exists()
            {
                var cache = Mock.Of<ICache>();
                var response = new HttpResponseMessage();
                IPrincipal assignedPrincipal = null;

                Mock.Get(cache)
                    .Setup(c => c.RetrieveAsync<IEnumerable<Subscriber>>(AclConfiguration.SubscribersCacheKey))
                    .Returns(Task.FromResult(new[]
                    {
                        GetSubscriber(secret: Secret)
                    }
                    .AsEnumerable()));

                SendAsync(
                    authorization: ValidAuthorizationHeader,
                    cache: cache,
                    assignPrincipalFactory: (r, p) => assignedPrincipal = p);

                Assert.IsNotNull(assignedPrincipal);
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

            private static Task<HttpResponseMessage> SendAsync(
                bool forceAuthentication = false,
                string authorization = null,
                ICache cache = null,
                HttpResponseMessage response = null,
                ISubscriberRepository subscriberRepository = null,
                Action<HttpRequestMessage, IPrincipal> assignPrincipalFactory = null)
            {
                return new HttpMessageInvoker(
                    CreateSubjectUnderTest(response, subscriberRepository, forceAuthentication, assignPrincipalFactory))
                    .SendAsync(
                        GetHttpRequestMessage(authorization, cache),
                        It.IsAny<CancellationToken>());
            }

            private static HttpRequestMessage GetHttpRequestMessage(
                string authorization,
                ICache cache)
            {
                var requestMessage = new HttpRequestMessage();

                if (!authorization.IsNullOrEmpty())
                    requestMessage.Headers.Add(
                        "Authorization", 
                        "Basic {0}".AsFormat(authorization));

                requestMessage.Properties[HttpPropertyKeys.HttpConfigurationKey] =
                    GetConfiguration(() => cache ?? Mock.Of<ICache>());

                return requestMessage;
            }

            private static HttpConfiguration GetConfiguration(
                Func<ICache> cacheRetriever)
            {
                var config = new HttpConfiguration();

                config.Properties.GetOrAdd(typeof(ICache), obj => cacheRetriever);

                return config;
            }

            private static AclBasedSecurityMessageHandler CreateSubjectUnderTest(
                HttpResponseMessage response,
                ISubscriberRepository subscriberRepository,
                bool forceAuthentication = false,
                Action<HttpRequestMessage, IPrincipal> assignPrincipalFactory = null)
            {
                IPrincipal principal;
                if (assignPrincipalFactory.IsNull())
                    assignPrincipalFactory = (r, p) => principal = p;

                var sut = new AclBasedSecurityMessageHandler(forceAuthentication);

                sut.InnerHandler = new TestHandler(response);
                sut.SetSubscriberRepositoryFactory(
                    (config, request) => subscriberRepository ?? Mock.Of<ISubscriberRepository>());
                sut.SetAssignPrincipalFactory(assignPrincipalFactory);

                return sut;
            }

            private static IEnumerable<Subscriber> GetSubscribers()
            {
                var numbersOfSubscribers = GetRand(10);

                for (var i = 0; i < numbersOfSubscribers; i++)
                    yield return GetSubscriber(ObjectMother.Create<string>());
            }

            private static int GetRand(int max)
            {
                return new Random().Next(1, max);
            }

            private static Subscriber GetSubscriber(
                string identifier = Subscriber,
                string secret = null)
            {
                return new Subscriber(identifier, secret);
            }
        }
    }
}
