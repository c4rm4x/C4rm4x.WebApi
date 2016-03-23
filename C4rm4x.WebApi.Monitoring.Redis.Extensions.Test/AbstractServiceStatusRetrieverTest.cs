#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Cache.Redis;
using C4rm4x.WebApi.Framework.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using System.Configuration;

#endregion

namespace C4rm4x.WebApi.Monitoring.Redis.Test
{
    public partial class AbstractServiceStatusRetrieverTest
    {
        private const string RedisConnectionString = "Redis";

        #region Helper classes

        public class TestServiceStatusRetriever :
            AbstractServiceStatusRetriever
        {
            public TestServiceStatusRetriever(
                ICache cache) :
                base("componentIdentifier", "componentName", cache)
            { }
        }

        #endregion

        private static string GetConnectionString()
        {
            return ConfigurationManager
                .ConnectionStrings[RedisConnectionString]
                .ConnectionString;
        }

        [TestClass]
        public abstract class AbstractServiceStatusRetrieverFixture :
            IntegrationFixture<TestServiceStatusRetriever>
        {
            public string ConnectionString { get; private set; }

            public AbstractServiceStatusRetrieverFixture(string connectionString = null)
            {
                ConnectionString = connectionString ?? GetConnectionString();
            }

            protected override void RegisterDependencies(
                Container container,
                Lifestyle lifeStyle)
            {
                container.Register(typeof(ICache), () => new RedisCache(ConnectionString), lifeStyle);

                base.RegisterDependencies(container, lifeStyle);
            }
        }
    }
}
