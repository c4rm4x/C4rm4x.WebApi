#region Using

using C4rm4x.WebApi.Persistance.Mongo.Test.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Persistance.Mongo.Test
{
    public partial class BaseRepositoryTest
    {
        private const string Value = "Value";
        private const string NonExistingId = "000000000000000000000000";

        [TestClass]
        public abstract class BaseRepositoryFixture :
            BasePersistanceFixture<TestRepository>
        {
            [TestInitialize]
            public override void Setup()
            {
                base.Setup();

                TestEntities.InsertMany(GetTestEntities().ToList());
            }

            private static IEnumerable<TestEntity> GetTestEntities()
            {
                for (int i = 1; i <= 10; i++)
                    yield return new TestEntity("" + i);
            }

            protected TestEntity GetEntityByValue(string value)
            {
                return TestEntities.Find(e => e.Value == value).FirstOrDefault();
            }
        }
    }
}
