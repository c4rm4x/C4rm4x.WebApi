#region Using

using C4rm4x.Tools.Utilities;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using SimpleInjector;

#endregion

namespace C4rm4x.WebApi.Scheduler.Quartz.SimpleInjector
{
    /// <summary>
    /// Base implementation of service responsible to configure
    /// and start the scheduler
    /// </summary>
    public static class Scheduler
    {
        private static IScheduler ThisScheduler;

        /// <summary>
        /// Configure the jobs to be scheduled and start the scheduler
        /// </summary>
        /// <param name="container">SimpleInjector container</param>
        /// <param name="configurations">All jobs configurations</param>
        public static void Start(
            Container container,
            params JobConfiguration[] configurations)
        {
            container.NotNull(nameof(container));
            configurations.NotNullOrEmpty(nameof(configurations));

            ThisScheduler = GetDefaultScheduler(container);

            foreach (var configuration in configurations)
                ThisScheduler.ScheduleJob(
                    JobBuilder.Create(configuration.JobType).Build(),
                    configuration.TriggerBuilder().Build());
        }

        private static IScheduler GetDefaultScheduler(
            Container container)
        {
            var scheduler = StdSchedulerFactory.GetDefaultScheduler();

            scheduler.JobFactory = container.GetInstance<IJobFactory>();

            scheduler.Start();

            return scheduler;
        }

        /// <summary>
        /// Stops the scheduler
        /// </summary>
        /// <param name="waitTillComplete">Wait for all jobs to complete before finishing</param>
        public static void Shutdown(bool waitTillComplete = true)
        {
            if (ThisScheduler.IsNull() || ThisScheduler.IsShutdown) return;

            ThisScheduler.Shutdown(waitTillComplete);
        }
    }
}
