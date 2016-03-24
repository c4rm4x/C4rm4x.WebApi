#region Using

using C4rm4x.WebApi.Persistance.EF;
using System.Data.Entity;

#endregion

namespace C4rm4x.WebApi.Monitoring.EF.Test
{
    public class TestEntities : ApiDbContext<TestEntities>
    {
        public TestEntities()
            : base("name=TestEntities")
        { }

        public DbSet<TestTable> TestTables { get; set; }
    }
}
