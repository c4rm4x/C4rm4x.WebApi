#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using SimpleInjector;
using System.Configuration;

#endregion

namespace C4rm4x.WebApi.Persistance.Mongo.Test.Infrastructure
{
    [TestClass]
    public abstract class BasePersistanceFixture<T>
        : IntegrationFixture<T>
        where T : class
    {
        private const string TestDatabase = "Test";

        [TestCleanup]
        public override void Cleanup()
        {
            Client.DropDatabase(TestDatabase);

            base.Cleanup();
        }

        protected override void RegisterDependencies(Container container, Lifestyle lifeStyle)
        {
            container.Register<MongoClient>(
                () => new MongoClient(ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString), lifeStyle);
            container.Register<IMongoDatabase>(
                () => container.GetInstance<MongoClient>().GetDatabase(TestDatabase), lifeStyle);

            base.RegisterDependencies(container, lifeStyle);
        }

        private MongoClient Client
        {
            get { return GetInstance<MongoClient>(); }
        }

        protected IMongoCollection<TestEntity> TestEntities
        {
            get
            {
                return GetInstance<IMongoDatabase>()
                    .GetCollection<TestEntity>(typeof(TestEntity).Name);
            }
        }
    }
}
