#region Using

using C4rm4x.Tools.Utilities;
using Quartz;
using System;

#endregion

namespace C4rm4x.WebApi.Scheduler.Quartz.SimpleInjector
{
    /// <summary>
    /// Represents an scheduled job configuration
    /// </summary>
    public class JobConfiguration
    {
        /// <summary>
        /// Gets the type of the job
        /// </summary>
        public Type JobType { get; private set; }

        /// <summary>
        /// Gets the trigger builder
        /// </summary>
        public Func<TriggerBuilder> TriggerBuilder { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="jobType">The type of the class that implements the job</param>
        /// <param name="builder">The trigger builder</param>
        public JobConfiguration(
            Type jobType,
            Func<TriggerBuilder> builder)
        {
            jobType.Is<IJob>();
            builder.NotNull(nameof(builder));

            JobType = jobType;
            TriggerBuilder = builder;
        }
    }
}
