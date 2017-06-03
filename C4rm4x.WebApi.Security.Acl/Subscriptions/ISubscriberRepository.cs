#region Using

using System.Linq;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Security.Acl.Subscriptions
{
    /// <summary>
    /// Subscriber repository
    /// </summary>
    public interface ISubscriberRepository
    {
        /// <summary>
        /// Get all the subscribers
        /// </summary>
        /// <returns>All the subscribers already registered</returns>
        Task<IQueryable<Subscriber>> GetAllAsync();
    }
}
