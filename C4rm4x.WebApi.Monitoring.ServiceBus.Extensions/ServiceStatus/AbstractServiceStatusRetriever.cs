#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Monitoring.ServiceBus.Core;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Threading.Tasks;
using BaseAbstractServiceStatusRetriever = C4rm4x.WebApi.Monitoring.ServiceStatus.AbstractServiceStatusRetriever;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceBus
{
    /// <summary>
    /// Basic implementation of IServiceStatusRetriever for ServiceBus
    /// </summary>
    public abstract class AbstractServiceStatusRetriever :
        BaseAbstractServiceStatusRetriever
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
        public AbstractServiceStatusRetriever(
            object componentIdentifier, 
            string componentName,
            string path,
            ITopicDescriptionRetriever topicDescriptionRetriever) : 
            base(componentIdentifier, componentName)
        {
            path.NotNullOrEmpty(nameof(path));
            topicDescriptionRetriever.NotNull(nameof(topicDescriptionRetriever));

            TopicPath = path;
            _topicDescriptionRetriever = topicDescriptionRetriever;
        }

        /// <summary>
        /// Checks whether or not the service bus topic is up and running
        /// </summary>
        protected override Task CheckComponentResponsivenessAsync()
        {
            return Task.Run(() =>
            {
                if (_topicDescriptionRetriever
                .Get(TopicPath)
                .AvailabilityStatus != EntityAvailabilityStatus.Available)
                    throw new ApplicationException("ServiceBus is not available");
            });
        }
    }
}
