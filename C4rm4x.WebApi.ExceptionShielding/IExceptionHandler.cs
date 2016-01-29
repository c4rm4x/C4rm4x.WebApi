#region Using

using System;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding
{
    /// <summary>
    /// Defines the contract for an ExceptionHandler.  
    /// An ExceptionHandler contains specific handling logic that is executed in a 
    /// chain of multiple ExceptionHandlers.  
    /// A chain of one or more ExceptionHandlers is executed based on the exception type being handled
    /// </summary>    
    public interface IExceptionHandler
    {
        /// <summary>
        /// Handles the exception
        /// </summary>
        /// <param name="exception">The exception to be handled</param>        
        /// <param name="handlingInstanceId">The unique ID attached to the handling chain for this handling instance</param>
        /// <returns>Modified exception to pass to the next exceptionHandler in the chain</returns>
        Exception HandleException(
            Exception exception,
            Guid handlingInstanceId);
    }
}
