#region Using

using System;

#endregion

namespace C4rm4x.WebApi.Framework.Messaging
{
    /// <summary>
    /// Service responsible to create transactions to add messages as a whole
    /// </summary>
    public interface IMessageTransactionFactory
    {
        /// <summary>
        /// Creates a new transaction as IDisposable object
        /// </summary>
        /// <returns>The new transaction</returns>
        /// <remarks>The new transaction will be open when created and closed when disposed</remarks>
        IDisposable Create();
    }
}
