#region Using

using System.Collections.Generic;

#endregion

namespace C4rm4x.WebApi.Security.WhiteList.Subscriptions
{
    /// <summary>
    /// Interface to retrieve all the subscriber insterested in this app
    /// </summary>
    public interface ISubscriptionDataProvider
    {
        /// <summary>
        /// Gets the list of all subscriber interested in this app
        /// </summary>
        IEnumerable<Subscriber> GetAll();
    }
}
