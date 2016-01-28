#region Using

using C4rm4x.WebApi.Framework;
using C4rm4x.WebApi.Framework.Messaging;
using System;
using System.Messaging;

#endregion

namespace C4rm4x.WebApi.Messaging.MSMQ
{
    /// <summary>
    /// Implementation of IMessageTransactionFactory using MessageQueueTransaction
    /// </summary>
    [DomainService(typeof(IMessageTransactionFactory))]
    public class MessageTransactionFactory : IMessageTransactionFactory
    {
        internal class MessageTransaction : MessageQueueTransaction
        {
            public MessageTransaction()
            {
                Begin();
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                    Commit();

                base.Dispose(disposing);
            }
        }

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
