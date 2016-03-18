#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Test.Controllers
{
    [TestClass]
    public abstract class AbstractServiceStatusHandlerTest
    {
        protected const string DefaultComponentIdentifier = "Identifier";
        protected const string DefaultComponentName = "Name";

        protected Task<HttpResponseMessage> Handle(
            CheckHealthRequest request = null,
            params IServiceStatusRetriever[] retrievers)
        {
            return CreateSubjetUnderTest(retrievers)
                .Handle(request ?? new CheckHealthRequestBuilder().Build())
                .ExecuteAsync(It.IsAny<CancellationToken>());
        }

        private IServiceStatusRequestHandler CreateSubjetUnderTest(
            params IServiceStatusRetriever[] retrievers)
        {
            var handler = GetServiceStatusRequestHandler() as IServiceStatusRequestHandler;

            handler.SetServiceStatusRetrievers(
                GetServiceStatusRetrievers(retrievers));

            return handler;
        }

        // That is why you should not test internal classes !
        protected abstract object GetServiceStatusRequestHandler();

        private static IEnumerable<IServiceStatusRetriever> GetServiceStatusRetrievers(
            params IServiceStatusRetriever[] retrievers)
        {
            return retrievers.IsNullOrEmpty()
                ? new[] { GetServiceStatusRetriever() }
                : retrievers;
        }

        protected static IServiceStatusRetriever GetServiceStatusRetriever(
            string componentIdentifier = DefaultComponentIdentifier,
            string componentName = DefaultComponentName)
        {
            var retriever = Mock.Of<IServiceStatusRetriever>();

            Mock.Get(retriever)
                .SetupGet(r => r.ComponentIdentifier)
                .Returns(componentIdentifier);

            Mock.Get(retriever)
                .SetupGet(r => r.ComponentName)
                .Returns(componentName);

            return retriever;
        }

        protected static IEnumerable<IServiceStatusRetriever> GetRetrievers()
        {
            var numberOfRetrievers = GetRand(10);

            for (var i = 0; i < numberOfRetrievers; i++)
                yield return GetServiceStatusRetriever();
        }

        protected static int GetRand(int max)
        {
            return new Random().Next(1, max);
        }
    }
}