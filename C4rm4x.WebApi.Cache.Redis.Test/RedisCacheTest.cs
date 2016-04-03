#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.Tools.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SimpleInjector;
using StackExchange.Redis;
using System;
using System.Configuration;
using System.Runtime.Serialization;

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

        protected static void AddEntry<TValue>(
            string key,
            TValue value)
            where TValue : class
        {
            Cache.StringSet(key, JsonConvert.SerializeObject(value));
        }

        protected static TestClass GetItem(string key)
        {
            string instance = Cache.StringGet(key);

            if (instance.IsNullOrEmpty()) return null;

            return JsonConvert.DeserializeObject<TestClass>(instance);
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
