#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Messaging;
using System.Messaging;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Messaging.MSMQ
{
    /// <summary>
    /// Implementation of IMessageQueueHandler using MSMQ library
    /// </summary>
    public abstract class MessageQueueHandler : IMessageQueueHandler
    {
        private MessageQueue _queue;
        private readonly IMessageTransactionFactory _transactionFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="transactionFactory">The transaction factory</param>
        public MessageQueueHandler(
            IMessageTransactionFactory transactionFactory)
        {
            transactionFactory.NotNull(nameof(transactionFactory));

            _transactionFactory = transactionFactory;
        }

        /// <summary>
        /// Sends a new item into the message queue
        /// </summary>
        /// <param name="item">The new item</param>
        public Task SendAsync<TItem>(TItem item)
            where TItem : class
        {
            return Task.Run(() =>
            {
                using (var transaction = _transactionFactory.Create() as MessageQueueTransaction)
                {
                    Queue.Send(item, transaction);
                }
            });
        }

        private MessageQueue Queue
        {
            get
            {
                if (_queue.IsNull())
                    _queue = GetMessageQueue(QueuePath, IsQueueTransactional);

                return _queue;
            }
        }

        private MessageQueue GetMessageQueue(
            string path,
            bool isTransactional = true)
        {
            var queue = MessageQueue.Exists(path)
                ? new MessageQueue(path)
                : CreateMessageQueue(path, isTransactional);

            SetQueueFormater(queue);

            return queue;
        }        

        private static MessageQueue CreateMessageQueue(
            string path,
            bool isTransactional)
        {
            const MessageQueueAccessRights ReadAccessRight = MessageQueueAccessRights.ReceiveMessage;
            const MessageQueueAccessRights WriteAccessRight = MessageQueueAccessRights.WriteMessage;
            var User = Thread.CurrentPrincipal.Identity.Name; // Let's pretend is a constant

            var queue = MessageQueue.Create(path, isTransactional);

            queue.SetPermissions(User, ReadAccessRight);
            queue.SetPermissions(User, WriteAccessRight);

            return queue;
        }

        /// <summary>
        /// Gets the path of the queue
        /// </summary>
        protected abstract string QueuePath { get; }

        /// <summary>
        /// Gets whether or not the queue is transactional
        /// </summary>
        protected abstract bool IsQueueTransactional { get; }

        /// <summary>
        /// Sets Queue formatter (is required)
        /// </summary>
        /// <param name="queue">The queue</param>
        protected abstract void SetQueueFormater(MessageQueue queue);
    }
}
