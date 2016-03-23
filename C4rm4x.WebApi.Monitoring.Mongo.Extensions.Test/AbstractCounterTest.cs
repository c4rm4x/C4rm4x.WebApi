#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Persistance.Mongo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using SimpleInjector;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Monitoring.Mongo.Test
{
    public partial class AbstractCounterTest
    {
        #region Helper classes

        public class TestEntity : BaseEntity
        {
            public string Value { get; set; }

            public TestEntity()
                : this(string.Empty)
            {
            }

            public TestEntity(string value)
            {
                Value = value;
            }
        }

        public class TestRepository : BaseRepository<TestEntity>
        {
            public TestRepository(IMongoDatabase database)
                : base(database)
            { }
        }

        public class TestCounter : AbstractCounter<TestEntity>
        {
            public TestCounter(TestRepository repository) 
                : base("componentIdentifier", "componentName", repository)
            {
            }
        }

        #endregion

        [TestClass]
        public abstract class AbstractCounterFixture :
            IntegrationFixture<TestCounter>
        {
            private const string TestDatabase = "Test";

            [TestInitialize]
            public override void Setup()
            {
                base.Setup();

                TestEntities.InsertMany(GetTestEntities().ToList());
            }

            [TestCleanup]
            public override void Cleanup()
            {
                Client.DropDatabase(TestDatabase);

                base.Cleanup();
            }

            protected override void RegisterDependencies(
                Container container, 
                Lifestyle lifeStyle)
            {
                container.Register<MongoClient>(
                    () => new MongoClient(ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString), lifeStyle);
                container.Register<IMongoDatabase>(
                    () => container.GetInstance<MongoClient>().GetDatabase(TestDatabase), lifeStyle);
                container.Register<TestRepository>(lifeStyle);

                base.RegisterDependencies(container, lifeStyle);
            }

            private MongoClient Client
            {
                get { return GetInstance<MongoClient>(); }
            }

            private IMongoCollection<TestEntity> TestEntities
            {
                get
                {
                    return GetInstance<IMongoDatabase>()
                        .GetCollection<TestEntity>(typeof(TestEntity).Name);
                }
            }

            private static IEnumerable<TestEntity> GetTestEntities()
            {
                for (int i = 1; i <= 10; i++)
                    yield return new TestEntity("" + i);
            }
        }
    }
}
