namespace C4rm4x.WebApi.Persistance.EF.Test.Infrastructure
{
    public class TestUnitOfWork : UnitOfWork
    {
        public TestUnitOfWork(TestEntities entities)
            : base(entities)
        {
        }
    }
}
