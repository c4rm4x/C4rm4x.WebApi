namespace C4rm4x.WebApi.Persistance.EF.Test.Infrastructure
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
