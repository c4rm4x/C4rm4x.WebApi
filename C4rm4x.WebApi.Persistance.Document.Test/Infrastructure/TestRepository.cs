#region Using

using Microsoft.Azure.Documents;

#endregion

namespace C4rm4x.WebApi.Persistance.Document.Test.Infrastructure
{
    public class TestRepository : BaseRepository<TestEntity>
    {
        public TestRepository(
            IDocumentClient client, 
            Database database) : base(client, database)
        {
        }
    }
}
