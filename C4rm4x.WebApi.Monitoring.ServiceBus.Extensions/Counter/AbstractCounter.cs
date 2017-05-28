#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Monitoring.Core;
using C4rm4x.WebApi.Monitoring.Counter;
using C4rm4x.WebApi.Monitoring.ServiceBus.Core;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceBus
{
    /// <summary>
    /// Base implementation of a counter for ServiceBus
    /// </summary>
    public abstract class AbstractCounter :
        AbstractMonitorService<long>,
        ICounter
    {
        /// <summary>
        /// Gets the topic path
        /// </summary>
        public string TopicPath { get; private set; }

        private readonly ITopicDescriptionRetriever _topicDescriptionRetriever;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="componentIdentifier">The component's identifier</param>
        /// <param name="componentName">The component's name</param>
        /// <param name="path">The topic path</param>
        /// <param name="topicDescriptionRetriever">The topic description retriever</param>
        public AbstractCounter(
            object componentIdentifier,
            string componentName,
            string path,
            ITopicDescriptionRetriever topicDescriptionRetriever)
            : base(componentIdentifier, componentName)
        {
            path.NotNullOrEmpty(nameof(path));
            topicDescriptionRetriever.NotNull(nameof(topicDescriptionRetriever));

            TopicPath = path;
            _topicDescriptionRetriever = topicDescriptionRetriever;
        }

        /// <summary>
        /// Counts the number of messages pending to be processed in the topic
        /// </summary>
        /// <returns>The total number of messages pending to be processed in the topic</returns>
        public override async Task<long> MonitorAsync()
        {
            return await Task.FromResult(DoMonitor());
        }

        private long DoMonitor()
        {
            try
            {
                return _topicDescriptionRetriever
                    .Get(TopicPath)
                    .SizeInBytes;
            }
            catch
            {
                return -1;
            }
        }
    }
}
