#region Using

using MongoDB.Driver;

#endregion

namespace C4rm4x.WebApi.Persistance.Mongo.Test.Infrastructure
{
    public class TestRepository : BaseRepository<TestEntity>
    {
        public TestRepository(IMongoDatabase database)
            : base(database)
        {
        }
    }
}
