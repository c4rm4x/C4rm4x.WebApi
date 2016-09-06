#region Using

using System;
using System.Transactions;

#endregion

namespace C4rm4x.WebApi.Messaging.ServiceBus
{
    internal class MessageTransaction : IDisposable
    {
        /// <summary>
        /// Gets the instance of the underlying scope
        /// </summary>
        public TransactionScope Scope { get; private set; }

        /// <summary>
        /// Gets whether or not the unit of work has already been disposed
        /// </summary>
        public bool IsDisposed { get; private set; }

        public MessageTransaction()
        {
            Scope = new TransactionScope();
        }

        public void Dispose()
        {
            if (IsDisposed) return;

            Scope.Complete();

            Scope.Dispose();

            IsDisposed = true;
        }
    }
}
