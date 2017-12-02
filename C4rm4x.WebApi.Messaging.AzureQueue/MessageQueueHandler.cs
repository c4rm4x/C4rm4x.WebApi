#region Using

using C4rm4x.Tools.AzureQueue;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Messaging;
using Microsoft.WindowsAzure.Storage;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Messaging.AzureQueue
{
    /// <summary>
    /// Implementation of IMessageQueueHandler using Azure Queues
    /// </summary>
    public class MessageQueueHandler : IMessageQueueHandler
    {
        private readonly CloudStorageAccount _cloudStorageAccount;

        private readonly IQueueReferenceFactory _queueReferenceFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cloudStorageAccount">The cloud storage account</param>
        /// <param name="queueReferenceFactory">The queue reference factory</param>
        public MessageQueueHandler(
            CloudStorageAccount cloudStorageAccount,
            IQueueReferenceFactory queueReferenceFactory)
        {
            cloudStorageAccount.NotNull(nameof(cloudStorageAccount));
            queueReferenceFactory.NotNull(nameof(queueReferenceFactory));

            _cloudStorageAccount = cloudStorageAccount;
            _queueReferenceFactory = queueReferenceFactory;
        }

        /// <summary>
        /// Sends a new item into the message queue
        /// </summary>
        /// <typeparam name="TItem">Type of the item</typeparam>
        /// <param name="item">The new item</param>
        public Task SendAsync<TItem>(TItem item)
            where TItem : class
        {
            return _cloudStorageAccount
                .CreateCloudQueueClient()
                .GetQueueReference(_queueReferenceFactory.Get())
                .AddMessageAsync(item.BuildCloudQueueMessage());
        }
    }
}
