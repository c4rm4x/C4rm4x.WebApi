#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Monitoring.ServiceBus.Core;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using System.Collections.Generic;
using System;
using System.Configuration;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceBus.Test
{
    [TestClass]
    public abstract class BaseServiceBusFixture<T>
        : IntegrationFixture<T>
        where T : class
    {
        public string TopicPath { get; private set; }

        public BaseServiceBusFixture(string topicPath)
            : base()
        {
            topicPath.NotNullOrEmpty(nameof(topicPath));

            TopicPath = topicPath;
        }

        protected override void RegisterDependencies(
            Container container, 
            Lifestyle lifeStyle)
        {
            base.RegisterDependencies(container, lifeStyle);

            container.Register<INamespaceManagerFactory>(
                () => new NamespaceManagerFactory(GetConnectionString()), 
                lifeStyle);
            container.Register<ITopicDescriptionRetriever, TopicDescriptionRetriever>(lifeStyle);
        }

        private static string GetConnectionString()
        {
            return ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
        }

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();

            CreateTopic();
        }

        private void CreateTopic()
        {
            var namespaceManager = NamespaceManager;

            if (!namespaceManager.TopicExists(TopicPath))
                namespaceManager.CreateTopic(TopicPath);
        }

        [TestCleanup]
        public override void Cleanup()
        {
            DeleteTopic();

            base.Cleanup();
        }

        private void DeleteTopic()
        {
            var namespaceManager = NamespaceManager;

            if (namespaceManager.TopicExists(TopicPath))
                namespaceManager.DeleteTopic(TopicPath);
        }

        protected NamespaceManager NamespaceManager
        {
            get { return GetInstance<INamespaceManagerFactory>().Get(); }
        }

        protected void PushMessages(IEnumerable<TestMessage> messages)
        {
            CreateSubscription();

            TopicClient.Create(TopicPath)
                .SendBatch(GetBrokeredMessages(messages));
        }

        private void CreateSubscription()
        {
            var namespaceManager = NamespaceManager;

            if (!namespaceManager.SubscriptionExists(TopicPath, "test"))
                namespaceManager.CreateSubscription(TopicPath, "test");
        }

        private IEnumerable<BrokeredMessage> GetBrokeredMessages(
            IEnumerable<TestMessage> messages)
        {
            foreach (var message in messages)
                yield return new BrokeredMessage(message);
        }
    }
}
