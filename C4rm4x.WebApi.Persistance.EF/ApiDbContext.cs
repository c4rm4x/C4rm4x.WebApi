#region Using

using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

#endregion

namespace C4rm4x.WebApi.Persistance.EF
{
    /// <summary>
    /// Custom implementation of DbContext for Api
    /// </summary>
    /// <remarks>
    /// The default behavior is not initializing the database under no circunstances
    /// and removes PluralizingTableNameConvention so the table names when no table name is specified
    /// </remarks>
    /// <typeparam name="TContext">Type of the db Context</typeparam>
    public abstract class ApiDbContext<TContext> : DbContext
        where TContext : DbContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nameOrConnectionString">Either the database name or a connection string</param>
        public ApiDbContext(string nameOrConnectionString = null)
            : base(nameOrConnectionString)
        {
            Database.SetInitializer<TContext>(null);
        }

        /// <summary>
        /// This method is called when the model for a derived context has been initialized,
        /// but before the model has been locked down and used to initialize the context 
        /// </summary>
        /// <param name="modelBuilder">The db model</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            ConfigureEntities(modelBuilder);
        }

        /// <summary>
        /// Configure using the FluentAPI the model
        /// </summary>
        /// <remarks>Default beviour: do nothing</remarks>
        /// <param name="modelBuilder">The model builder</param>
        protected virtual void ConfigureEntities(DbModelBuilder modelBuilder)
        {
        }
    }
}
