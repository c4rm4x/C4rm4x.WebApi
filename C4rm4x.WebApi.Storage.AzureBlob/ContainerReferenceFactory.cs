#region Using

using C4rm4x.Tools.Utilities;

#endregion

namespace C4rm4x.WebApi.Storage.AzureBlob
{
    /// <summary>
    /// Interface to return the container reference
    /// </summary>
    public interface IContainerReferenceFactory
    {
        /// <summary>
        /// Gets the container name
        /// </summary>
        string Get();
    }

    /// <summary>
    /// Base implementation of IContainerReferenceFactory
    /// </summary>
    public class ContainerReferenceFactory : IContainerReferenceFactory
    {
        /// <summary>
        /// Gets the container name
        /// </summary>
        public string ContainerName { private set; get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="containerName">The container name</param>
        public ContainerReferenceFactory(string containerName)
        {
            containerName.NotNullOrEmpty(nameof(containerName));

            ContainerName = containerName;
        }

        /// <summary>
        /// Gets the container name
        /// </summary>
        public string Get() => ContainerName;
    }
}
