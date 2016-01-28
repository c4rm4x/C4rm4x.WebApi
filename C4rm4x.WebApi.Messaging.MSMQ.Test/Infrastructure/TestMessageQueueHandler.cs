#region Using

using C4rm4x.WebApi.Framework.Messaging;
using System;
using System.Messaging;

#endregion

namespace C4rm4x.WebApi.Messaging.MSMQ.Test.Infrastructure
{
    public class TestMessageQueueHandler : MessageQueueHandler
    {
        public const string TestQueuePath = @".\private$\test";

        public TestMessageQueueHandler(
            IMessageTransactionFactory transactionFactory)
            : base(transactionFactory)
        { }

        protected override string QueuePath
        {
            get { return TestQueuePath; }
        }

        protected override bool IsQueueTransactional
        {
            get { return true; }
        }

        protected override void SetQueueFormater(MessageQueue queue)
        {
            queue.Formatter = 
                new XmlMessageFormatter(
                    new Type[] {
                        typeof(TestSelfProcessedMessage) });
        }
    }
}
