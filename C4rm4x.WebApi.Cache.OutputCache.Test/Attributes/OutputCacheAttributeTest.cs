#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;

#endregion

namespace C4rm4x.WebApi.Cache.OutputCache.Test
{
    public partial class OutputCacheAttributeTest
    {
        [TestClass]
        public abstract class OutputCacheAttributeFixture
        {
            protected const string Key = "key";

            #region Helper classes

            private class TestCacheKeyGenerator : ICacheKeyGenerator
            {
                public string Generate(HttpActionContext actionContext)
                {
                    return Generate(actionContext, null);
                }

                public string Generate(
                    Type controllerType, 
                    string actionName,
                    HttpActionContext actionContext)
                {
                    return Generate(null, null);
                }

                public string Generate(
                    HttpActionContext actionContext, 
                    string actionName)
                {
                    return Key;
                }
            }

            #endregion

            protected static OutputCacheAttribute CreateSubjectUnderTest(
                int serverTimeSpan = 1,
                int clientTimeSpan = 0,
                Type keyGeneratorType = null)
            {
                return new OutputCacheAttribute(
                    serverTimeSpan,
                    clientTimeSpan,
                    keyGeneratorType ?? typeof(TestCacheKeyGenerator));
            }

            protected static ICache GetCache()
            {
                return Mock.Of<ICache>();
            }

            protected static HttpActionContext GetHttpActionContext(
                ICache cache)
            {
                return GetHttpActionContext(cache, HttpMethod.Get);
            }

            protected static HttpActionContext GetHttpActionContext(
                ICache cache,
                HttpMethod method)
            {
                return new HttpActionContext(
                    GetHttpControllerContext(cache, method),
                    GetHttpActionDescriptor());
            }

            private static HttpControllerContext GetHttpControllerContext(
                ICache cache,
                HttpMethod method)
            {
                var config = GetHttpConfiguration(() => cache);
                var request = GetHttpRequestMessage(method, config);

                return new HttpControllerContext(
                    config,
                    GetHttpRouteData(config, request),
                    request);
            }

            private static HttpConfiguration GetHttpConfiguration(
                Func<ICache> cacheRetriever)
            {
                var config = new HttpConfiguration();

                config.Properties.GetOrAdd(typeof(ICache), obj => cacheRetriever);

                return config;
            }   
            
            private static HttpRouteData GetHttpRouteData(
                HttpConfiguration config,
                HttpRequestMessage request)
            {                
                var route = config.Routes.MapHttpRoute(
                    "default", "api/{controller}/{id}");

                return new HttpRouteData(
                    route, 
                    new HttpRouteValueDictionary { { "controller", "test" } });
            }         

            private static HttpRequestMessage GetHttpRequestMessage(
                HttpMethod method,
                HttpConfiguration config)
            {
                var request = new HttpRequestMessage(
                    method, "http://localhost/api/test");

                request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

                return request;
            }

            private static HttpActionDescriptor GetHttpActionDescriptor()
            {
                return Mock.Of<HttpActionDescriptor>();
            }

            protected static bool AreEqual(
                byte[] expected,
                byte[] actual)
            {
                if (expected.Length != actual.Length)
                    return false;

                for (var i = 0; i < expected.Length; i++)
                    if (expected[i] != actual[i]) return false;

                return true;
            }

            protected static IEnumerable<byte> GetContent()
            {
                var size = GetRand(100);

                for (var i = 0; i < size; i++)
                    yield return ObjectMother.Create<byte>();
            }

            private static int GetRand(int max)
            {
                return new Random().Next(1, max);
            }
        }
    }
}