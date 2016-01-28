#region System;

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.Messaging;
using C4rm4x.WebApi.Messaging.MSMQ.Test.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using System;
using System.Linq;
using System.Messaging;

#endregion

namespace C4rm4x.WebApi.Messaging.MSMQ.Test
{
    public partial class MessageQueueHandlerTest
    {
        [TestClass]
        public class MessageQueueHandlerSendTest
            : IntegrationFixture<TestMessageQueueHandler>
        {
            [TestCleanup]
            public override void Cleanup()
            {
                DeleteAllMessages();

                base.Cleanup();
            }

            [TestMethod, IntegrationTest]
            public void Send_Creates_A_New_Message()
            {
                _sut.Send(new TestSelfProcessedMessage(ObjectMother.Create<string>()));

                var messages = GetAllMessages();

                Assert.IsNotNull(messages);
                Assert.IsTrue(messages.Any());
                Assert.IsNotNull(messages.FirstOrDefault());
            }

            [TestMethod, IntegrationTest]
            public void Send_Creates_A_New_Message_With_Body_As_InstanceOf_SelfProcessedMessage()
            {
                var Body = ObjectMother.Create<string>();

                _sut.Send(new TestSelfProcessedMessage(Body));

                var message = GetAllMessages().First();

                Assert.IsNotNull(message.Body);
                Assert.IsInstanceOfType(message.Body, typeof(TestSelfProcessedMessage));
                Assert.AreEqual(Body, (message.Body as TestSelfProcessedMessage).Message);
            }

            private static MessageQueue GetMessageQueue()
            {
                var queue = new MessageQueue(
                    TestMessageQueueHandler.TestQueuePath);

                queue.Formatter = new XmlMessageFormatter(
                    new Type[] { typeof(TestSelfProcessedMessage) }); ;

                return queue;
            }

            private static Message[] GetAllMessages()
            {
                return GetMessageQueue().GetAllMessages();
            }

            private static void DeleteAllMessages()
            {
                GetMessageQueue().Purge();
            }

            protected override void RegisterDependencies(
                Container container,
                Lifestyle lifeStyle)
            {
                container
                    .Register<IMessageTransactionFactory, MessageTransactionFactory>(lifeStyle);
            }
        }
    }
}
