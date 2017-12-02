#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Messaging.AzureQueue.Test.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using SimpleInjector;
using System.Configuration;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Messaging.AzureQueue.Test
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

                container.Register<IQueueReferenceFactory>(() =>
                     new QueueReferenceFactory("test"), lifeStyle);
                container.Register(() =>
                    CloudStorageAccount.Parse(ConnectionString), lifeStyle);
            }

            private static string ConnectionString =>
                ConfigurationManager.ConnectionStrings["AzureConnection"].ConnectionString;

            [TestMethod, IntegrationTest]
            public async Task Send_Creates_A_New_Message_With_The_Body_Specified()
            {
                await _sut.SendAsync(new TestMessage(ObjectMother.Create<string>()));
            }            
        }
    }
}
