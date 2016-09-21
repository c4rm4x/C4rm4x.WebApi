namespace C4rm4x.WebApi.Framework
{
    /// <summary>
    /// Service to run scheduled tasks
    /// </summary>
    public interface IProcessor
    {
        /// <summary>
        /// Process the scheduled task
        /// </summary>
        void Process();
    }
}
