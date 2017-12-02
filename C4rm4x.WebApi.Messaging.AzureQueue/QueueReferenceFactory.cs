#region Using

using C4rm4x.Tools.Utilities;

#endregion

namespace C4rm4x.WebApi.Messaging.AzureQueue
{
    #region Interface

    /// <summary>
    /// Interface to return the queue reference
    /// </summary>
    public interface IQueueReferenceFactory
    {
        /// <summary>
        /// Gets the queue name
        /// </summary>
        string Get();
    }

    #endregion

    /// <summary>
    /// Base implementation of IQueueReferenceFactory
    /// </summary>
    public class QueueReferenceFactory : IQueueReferenceFactory
    {
        /// <summary>
        /// Gets the queue name
        /// </summary>
        public string QueueName { private set; get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="queueName">The queue name</param>
        public QueueReferenceFactory(string queueName)
        {
            queueName.NotNullOrEmpty(nameof(queueName));

            QueueName = queueName;
        }

        /// <summary>
        /// Gets the container name
        /// </summary>
        public string Get() => QueueName;
    }
}
