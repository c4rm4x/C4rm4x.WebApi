#region Using

using C4rm4x.Tools.Utilities;
using Microsoft.ServiceBus.Messaging;
using System;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceBus.Core
{
    #region Interface

    /// <summary>
    /// Service responsible to retrieve the topic description
    /// </summary>
    public interface ITopicDescriptionRetriever
    {
        /// <summary>
        /// Gets the topic description for the given one
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>The instance of TopicDescription</returns>
        /// <exception cref="ArgumentException">When no topic exists for the given path</exception>
        TopicDescription Get(string path);
    }

    #endregion

    /// <summary>
    /// Implemenattion of ITopicDescriptionRetriever
    /// </summary>
    public class TopicDescriptionRetriever : ITopicDescriptionRetriever
    {
        private readonly INamespaceManagerFactory _namespaceManagerFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="namespaceManagerFactory"></param>
        public TopicDescriptionRetriever(
            INamespaceManagerFactory namespaceManagerFactory)
        {
            namespaceManagerFactory.NotNull(nameof(namespaceManagerFactory));

            _namespaceManagerFactory = namespaceManagerFactory;
        }

        /// <summary>
        /// Gets the topic description for the given one
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>The instance of TopicDescription</returns>
        /// <exception cref="ArgumentException">When no topic exists for the given path</exception>
        public TopicDescription Get(string path)
        {
            path.NotNullOrEmpty(nameof(path));

            var namespaceManager = _namespaceManagerFactory.Get();

            if (!namespaceManager.TopicExists(path))
                throw new ArgumentException(
                    "There is no topic with the given name {0}".AsFormat(path));

            return namespaceManager.GetTopic(path);
        }
    }
}
