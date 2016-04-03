#region Using

using System;

#endregion

namespace C4rm4x.WebApi.Cache.OutputCache.Internals
{
    internal class CacheTime
    {
        internal CacheTime()
        { }

        public CacheTime(
            DateTimeOffset absoluteExpirationTime,
            TimeSpan clientTimeSpan)
        {
            AbsoluteExpirationTime = absoluteExpirationTime;
            ClientTimeSpan = clientTimeSpan;
        }

        public DateTimeOffset AbsoluteExpirationTime { get; private set; }

        public TimeSpan ClientTimeSpan { get; private set; }

        public static CacheTime From(
            int serverTimeSpan,
            int clientTimeSpan,
            DateTime now)
        {
            return new CacheTime(
                now.AddSeconds(serverTimeSpan),
                TimeSpan.FromSeconds(clientTimeSpan));
        }
    }
}
