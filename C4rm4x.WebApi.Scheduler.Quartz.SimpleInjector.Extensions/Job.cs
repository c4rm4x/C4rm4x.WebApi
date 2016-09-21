#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework;
using Quartz;
using SimpleInjector;

#endregion

namespace C4rm4x.WebApi.Scheduler.Quartz.SimpleInjector
{
    /// <summary>
    /// Implements interface IJob which represent a job to be performed based on given schedule
    /// </summary>
    /// <typeparam name="TProcessor">Type of the class that implements IProcessor</typeparam>
    public abstract class Job<TProcessor> : IJob
        where TProcessor : class, IProcessor
    {
        private readonly Container _container;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">SimpleInjector container</param>
        public Job(
            Container container)
        {
            container.NotNull(nameof(container));

            _container = container;
        }

        /// <summary>
        /// Called by the Quartz.IScheduler when a Quartz.ITrigger fires that is associated 
        /// with the Quartz.IJob
        /// </summary>
        /// <param name="context">The execution context</param>
        public void Execute(IJobExecutionContext context)
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                _container.GetInstance<TProcessor>().Process();
            }
        }
    }
}