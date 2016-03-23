#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StackExchange.Redis;
using System;
using System.Configuration;
using System.Runtime.Serialization;
using SimpleInjector;

#endregion

namespace C4rm4x.WebApi.Cache.Redis.Test
{
    public partial class RedisCacheTest
    {
        private const string RedisConnectionString = "Redis";

        #region Helper classes

        [DataContract]
        public class TestClass
        {
            [DataMember]
            public string Property { get; set; }

            public override bool Equals(object obj)
            {
                var objAsTestClass = obj as TestClass;

                if (objAsTestClass.IsNull()) return false;

                if (object.ReferenceEquals(this, obj))
                    return true;

                return this.Property == objAsTestClass.Property;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        #endregion

        private static Lazy<ConnectionMultiplexer> Connection =>
            new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(GetConnectionString());
            });

        private static string GetConnectionString()
        {            
            return ConfigurationManager
                .ConnectionStrings[RedisConnectionString]
                .ConnectionString;
        }

        protected static IDatabase Cache
        {
            get { return Connection.Value.GetDatabase(); }
        }

        [TestClass]
        public abstract class RedisCacheTestFixture : 
            IntegrationFixture<RedisCache>
        {
            protected override void RegisterDependencies(
                Container container, 
                Lifestyle lifeStyle)
            {
                container.Register(() => new RedisCache(GetConnectionString()), lifeStyle);
            }

            [TestCleanup]
            public override void Cleanup()
            {
                FlushCache();

                base.Cleanup();
            }

            private static void FlushCache()
            {
                var multiplexer = Cache.Multiplexer;

                foreach (var endpoint in multiplexer.GetEndPoints(true))
                    multiplexer.GetServer(endpoint).FlushAllDatabases();
            }
        }
    }
}
