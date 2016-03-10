#region Using

using C4rm4x.Tools.Utilities;
using System;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus
{
    /// <summary>
    /// Basic implementation of IServiceStatusRetriever
    /// </summary>
    public abstract class AbstractServiceStatusRetriever :
        IServiceStatusRetriever
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="componentIdentifier">The component's identifier</param>
        /// <param name="componentName">The component's name</param>
        public AbstractServiceStatusRetriever(
            object componentIdentifier,
            string componentName)
        {
            componentIdentifier.NotNull(nameof(componentIdentifier));
            componentName.NotNullOrEmpty(nameof(componentName));

            ComponentIdentifier = componentIdentifier;
            ComponentName = componentName;
        }

        /// <summary>
        /// The component's identifier which this service is responsible for
        /// </summary>
        public object ComponentIdentifier { get; private set; }

        /// <summary>
        /// The component's name with this service is responsible for
        /// </summary>
        public string ComponentName { get; private set; }

        /// <summary>
        /// Is the component working as expected?
        /// </summary>
        /// <returns>True when component is working as expected; false, otherwise</returns>
        public bool IsComponentWorking()
        {            
            try
            {
                CheckComponentResponsiveness();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks whether or not the component is responding as expected
        /// </summary>
        /// <remarks>DO THROW an exception when the component is not working as expected</remarks>
        protected abstract void CheckComponentResponsiveness();
    }
}