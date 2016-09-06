#region Using

using C4rm4x.WebApi.Persistance.Document.Test.Infrastructure;
using Microsoft.Azure.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Persistance.Document.Test
{
    public partial class BaseRepositoryTest
    {
        private const string Value = "Value";
        private const string NonExistingId = "00000000-0000-0000-0000-000000000000";

        [TestClass]
        public abstract class BaseRepositoryFixture :
            BasePersistenceFixture<TestRepository>
        {
            [TestInitialize]
            public override void Setup()
            {
                base.Setup();

                AddTestEntities();
            }

            private void AddTestEntities()
            {
                var entities = GetTestEntities().ToList();

                foreach (var entity in entities)
                    entity.Id = Client.CreateDocumentAsync(Collection.DocumentsLink, entity).Result.Resource.Id;
            }

            private static IEnumerable<TestEntity> GetTestEntities()
            {
                for (int i = 1; i <= 10; i++)
                    yield return new TestEntity("" + i);
            }

            protected override void RegisterDependencies(
                Container container, 
                Lifestyle lifeStyle)
            {
                base.RegisterDependencies(container, lifeStyle);

                container.Register<IDatabaseInitialiser, DatabaseInitialiser>(lifeStyle);

                container.Register(
                    () => container.GetInstance<IDatabaseInitialiser>().GetOrCreateDatabase(TestDatabase), 
                    lifeStyle);
            }

            protected DocumentCollection Collection
            {
                get { return GetOrCreateCollection().Result; }
            }

            private async Task<DocumentCollection> GetOrCreateCollection()
            {
                var collectionName = typeof(TestEntity).Name;

                var collection = Client
                    .CreateDocumentCollectionQuery(Database.SelfLink)
                    .AsEnumerable()
                    .FirstOrDefault(c => c.Id == collectionName);

                return collection ??
                    await Client.CreateDocumentCollectionAsync(
                        Database.SelfLink,
                        new DocumentCollection { Id = collectionName });
            }

            protected TestEntity GetEntityByValue(string value)
            {
                return Client
                    .CreateDocumentQuery<TestEntity>(Collection.DocumentsLink)
                    .Where(e => e.Value == value)
                    .AsEnumerable()
                    .FirstOrDefault();
            }
        }
    }
}
