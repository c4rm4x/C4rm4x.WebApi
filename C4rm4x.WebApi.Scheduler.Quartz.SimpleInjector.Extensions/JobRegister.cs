#region Using

using C4rm4x.WebApi.Framework;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#endregion

namespace C4rm4x.WebApi.Scheduler.Quartz.SimpleInjector
{
    /// <summary>
    /// Service responsible to register every class
    /// decorated with the attribute Job
    /// </summary>
    public static class JobRegister
    {
        /// <summary>
        /// Registers all the classes decorated with attribute
        /// Job within all the specified assemblies
        /// </summary>
        /// <param name="container"></param>
        /// <param name="assemblies"></param>
        public static void Register(
            Container container,
            params Assembly[] assemblies)
        {
            var jobTypes = GetJobTypes(assemblies);

            foreach (var jobType in jobTypes)
                container.Register(jobType);
        }

        private static IEnumerable<Type> GetJobTypes(
            params Assembly[] assemblies)
        {
            return assemblies
                .SelectMany(assembly => assembly.GetExportedTypes())
                .Where(type =>
                    type.GetCustomAttributes<JobAttribute>().Any() &&
                    !type.IsInterface &&
                    !type.IsAbstract)
                .ToList();                    
        }
    }
}
