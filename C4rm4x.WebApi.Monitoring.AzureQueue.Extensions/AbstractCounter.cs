#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Monitoring.Core;
using C4rm4x.WebApi.Monitoring.Counter;
using Microsoft.WindowsAzure.Storage;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Monitoring.AzureQueue
{
    /// <summary>
    /// Base implementation of a counter for Azure Queue
    /// </summary>
    public abstract class AbstractCounter :
        AbstractMonitorService<long>,
        ICounter
    {
        /// <summary>
        /// Gets the queue name
        /// </summary>
        public string QueueName { get; private set; }

        /// <summary>
        /// Gets the Azure cloud storage account
        /// </summary>
        public CloudStorageAccount CloudStorageAccount { get; private set; }        

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="componentIdentifier">The component's identifier</param>
        /// <param name="componentName">The component's name</param>
        /// <param name="queueName">The queue name</param>
        /// <param name="cloudStorageAccount">The cloud storage account</param>
        public AbstractCounter(
            object componentIdentifier,
            string componentName,
            string queueName,
            CloudStorageAccount cloudStorageAccount)
            : base(componentIdentifier, componentName)
        {
            queueName.NotNullOrEmpty(nameof(queueName));
            cloudStorageAccount.NotNull(nameof(cloudStorageAccount));

            QueueName = queueName;
            CloudStorageAccount = cloudStorageAccount;
        }

        /// <summary>
        /// Counts the number of messages pending to be processed in the queue
        /// </summary>
        /// <returns>The total number of messages pending to be processed in the queue</returns>
        public override async Task<long> MonitorAsync()
        {
            try
            {
                var queue = CloudStorageAccount
                    .CreateCloudQueueClient()
                    .GetQueueReference(QueueName);

                await queue.FetchAttributesAsync();

                return queue.ApproximateMessageCount.GetValueOrDefault();
            }
            catch
            {
                return -1;
            }
        }
    }
}
