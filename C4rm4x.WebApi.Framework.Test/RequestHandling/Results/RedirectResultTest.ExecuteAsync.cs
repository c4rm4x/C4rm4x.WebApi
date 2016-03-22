#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.RequestHandling.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Test.RequestHandling.Results
{
    public partial class RedirectResultTest
    {
        [TestClass]
        public class RedirectResultExecuteAsyncTest
        {
            private const string Target = "http://www.google.com";

            [TestMethod, UnitTest]
            public void ExecuteAsync_Returns_Redirect_Response()
            {
                Assert.AreEqual(
                    HttpStatusCode.Redirect,
                    ExecuteAsync(Target).Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void ExecuteAsync_Returns_HttpResponseMessage_With_Location_Header_As_Target_Url()
            {
                Assert.AreEqual(
                    new Uri(Target),
                    ExecuteAsync(Target).Result.Headers.Location);
            }

            [TestMethod, UnitTest]
            public void ExecuteAsync_Returns_HttpResponseMessage_With_Location_Header_As_Target_Url_With_QueryString()
            {
                var queryString = GetQueryString().ToArray();

                Assert.AreEqual(
                    new Uri(GetUrl(queryString)),
                    ExecuteAsync(Target, queryString).Result.Headers.Location);
            }

            private static RedirectResult CreateSubjectUnderTest(
                string url,
                params KeyValuePair<string, object>[] queryString)
            {
                return new RedirectResult(url, queryString);
            }

            private static Task<HttpResponseMessage> ExecuteAsync(
                string url,
                params KeyValuePair<string, object>[] queryString)
            {
                return CreateSubjectUnderTest(url, queryString)
                    .ExecuteAsync(It.IsAny<CancellationToken>());
            }

            private static IEnumerable<KeyValuePair<string, object>> GetQueryString()
            {
                var numberOfParameters = GetRand(10);

                for (var i = 0; i < numberOfParameters; i++)
                    yield return new KeyValuePair<string, object>(
                        ObjectMother.Create(10), ObjectMother.Create(100));
            }

            private static int GetRand(int max)
            {
                return new Random().Next(1, max);
            }

            private static string GetUrl(KeyValuePair<string, object>[] parameters)
            {
                return "{0}?{1}".AsFormat(
                    Target,
                    string.Join("&", parameters.Select(p => "{0}={1}".AsFormat(p.Key, p.Value))));
            }
        }
    }
}
