#region Using

using System.Data.Entity;

#endregion

namespace C4rm4x.WebApi.Persistance.EF.Test.Infrastructure
{
    public class TestEntities : ApiDbContext<TestEntities>
    {
        public TestEntities()
            : base("name=TestEntities")
        { }

        public DbSet<TestTable> TestTables { get; set; }
    }
}
