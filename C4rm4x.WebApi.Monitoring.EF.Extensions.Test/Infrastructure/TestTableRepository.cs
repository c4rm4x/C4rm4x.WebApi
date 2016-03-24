#region Using

using C4rm4x.WebApi.Persistance.EF;

#endregion

namespace C4rm4x.WebApi.Monitoring.EF.Test
{
    public class TestTableRepository :
        BaseRepository<TestTable, TestEntities>
    {
        public TestTableRepository(TestEntities entities)
            : base(entities)
        {
        }
    }
}
