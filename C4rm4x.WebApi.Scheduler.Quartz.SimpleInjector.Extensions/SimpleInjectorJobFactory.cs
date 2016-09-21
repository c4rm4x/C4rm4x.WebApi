#region Using

using C4rm4x.Tools.Utilities;
using Quartz;
using Quartz.Spi;
using SimpleInjector;

#endregion

namespace C4rm4x.WebApi.Scheduler.Quartz.SimpleInjector
{

    /// <summary>
    /// Service responsible for producing instances of Quartz.IJob classes
    /// using SimpleInjector container
    /// </summary>
    public class SimpleInjectorJobFactory : IJobFactory
    {
        private readonly Container _container;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">The SimpleInjector container</param>
        public SimpleInjectorJobFactory(
            Container container)
        {
            container.NotNull(nameof(container));

            _container = container;
        }

        /// <summary>
        /// Called by the scheduler at the time of the trigger firing, in order to produce
        /// a Quartz.IJob instance on which to call Execute
        /// </summary>
        /// <param name="bundle">The TriggerFiredBundle from which the Quartz.IJobDetail and other info relating to the trigger firing can be obtained.</param>
        /// <param name="scheduler">A handle to the scheduler that is about to execute the job</param>
        /// <returns>The new intance of IJob</returns>
        public IJob NewJob(
            TriggerFiredBundle bundle,
            IScheduler scheduler)
        {
            return (IJob)_container
                .GetInstance(bundle.JobDetail.JobType);
        }

        /// <summary>
        /// Allows the job factory to destroy/cleanup the job if needed
        /// </summary>
        /// <param name="job">The job</param>
        public void ReturnJob(IJob job)
        {
            // Nothing to do
        }
    }
}
