#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Messaging.ServiceBus.Test.Infrastructure;
using Microsoft.ServiceBus.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using System.Configuration;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Messaging.ServiceBus.Test
{
    public partial class MessageQueueHandlerTest
    {
        [TestClass]
        public class MessageQueueHandlerSendTest :
            IntegrationFixture<MessageQueueHandler>
        {
            protected override void RegisterDependencies(Container container, Lifestyle lifeStyle)
            {
                base.RegisterDependencies(container, lifeStyle);

                container.Register<ITopicClientFactory>(
                    () => new SimpleTopicClientFactory(
                        TopicClient.CreateFromConnectionString(ConnectionString, Path)),
                    lifeStyle);
            }

            [TestMethod, IntegrationTest]
            public async Task Send_Creates_A_New_Message_With_The_Body_Specified()
            {
                await _sut.SendAsync(new TestMessage(ObjectMother.Create<string>()));
            }

            private static string ConnectionString => GetSettings("ServiceBus.Settings.ConnectionString");

            private static string Path => GetSettings("ServiceBus.Settings.Path");

            private static string GetSettings(string key)
            {
                return ConfigurationManager.AppSettings[key];
            }
        }
    }
}
