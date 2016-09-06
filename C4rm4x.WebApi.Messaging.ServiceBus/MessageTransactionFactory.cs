#region Using

using C4rm4x.WebApi.Framework;
using C4rm4x.WebApi.Framework.Messaging;
using System;

#endregion

namespace C4rm4x.WebApi.Messaging.ServiceBus
{

    /// <summary>
    /// Implementation of IMessageTransactionFactory using TransactionScope
    /// </summary>
    [DomainService(typeof(IMessageTransactionFactory))]
    public class MessageTransactionFactory : IMessageTransactionFactory
    {       
        /// <summary>
        /// Creates a new transaction as IDisposable object
        /// </summary>
        /// <returns>The new transaction</returns>
        /// <remarks>The new transaction will be open when created and closed when disposed</remarks>
        public IDisposable Create()
        {
            return new MessageTransaction();
        }
    }
}
