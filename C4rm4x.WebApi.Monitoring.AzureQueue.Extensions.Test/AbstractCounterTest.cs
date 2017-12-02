#region Using

using C4rm4x.Tools.AzureQueue;
using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using SimpleInjector;
using System.Collections.Generic;
using System.Configuration;

#endregion

namespace C4rm4x.WebApi.Monitoring.AzureQueue.Test
{
    public partial class AbstractCounterTest
    {
        #region Helper classes

        public class TestCounter :
            AbstractCounter
        {
            public TestCounter(CloudStorageAccount account) :
                base("testCounter", "testCounter", "test", account)
            {
            }
        }

        #endregion

        [TestClass]
        public abstract class AbstractCounterFixture :
            IntegrationFixture<TestCounter>
        {
            protected override void RegisterDependencies(Container container, Lifestyle lifeStyle)
            {
                base.RegisterDependencies(container, lifeStyle);

                container.Register(() =>
                    CloudStorageAccount.Parse(ConnectionString), lifeStyle);
            }

            private static string ConnectionString =>
                ConfigurationManager.ConnectionStrings["AzureConnection"].ConnectionString;

            protected void PushMessages(IEnumerable<TestMessage> messages)
            {
                var account = GetInstance<CloudStorageAccount>();

                var queue = account.CreateCloudQueueClient().GetQueueReference("test");

                foreach (var message in messages)
                    queue.AddMessage(message.BuildCloudQueueMessage());
            }
        }
    }
}
