#region Using

using C4rm4x.WebApi.Framework.Persistance;

#endregion

namespace C4rm4x.WebApi.Security.Acl.Subscriptions
{
    /// <summary>
    /// Subscriber repository
    /// </summary>
    public interface ISubscriberRepository : 
        IRepository<Subscriber>
    {
    }
}
