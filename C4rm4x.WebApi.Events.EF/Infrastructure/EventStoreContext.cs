using C4rm4x.WebApi.Events.EF.Infrastructure.EntitiesConfiguration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace C4rm4x.WebApi.Events.EF.Infrastructure
{
    /// <summary>
    /// The event store context
    /// </summary>
    public abstract class EventStoreContext : DbContext
    {
        /// <summary>
        /// This method is called when the model for a derived context has been initialized,
        /// but before the model has been locked down and used to initialize the context 
        /// </summary>
        /// <param name="modelBuilder">The db model</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            Configure(modelBuilder);
        }

        private void Configure(DbModelBuilder modelBuilder) => Configurator.Configure(modelBuilder);

        // DbSets
        public virtual DbSet<Event> Events { get; set; }
    }
}