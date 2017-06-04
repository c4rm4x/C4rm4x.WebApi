#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Persistance.EF;
using System.Data.Entity;
using System.Threading.Tasks;
using BaseAbstractServiceStatusRetriever = C4rm4x.WebApi.Monitoring.ServiceStatus.AbstractServiceStatusRetriever;

#endregion

namespace C4rm4x.WebApi.Monitoring.EF
{
    /// <summary>
    /// Basic implementation of IServiceStatusRetriever using EF
    /// </summary>
    /// <typeparam name="TContext">Type of DBContext</typeparam>
    public abstract class AbstractServiceStatusRetriever<TContext> :
        BaseAbstractServiceStatusRetriever
        where TContext : DbContext
    {
        private readonly ApiDbContext<TContext> _entities;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="componentIdentifier">The component's identifier</param>
        /// <param name="componentName">The component's name</param>
        /// <param name="entities">The dbcontext</param>
        public AbstractServiceStatusRetriever(
            object componentIdentifier, 
            string componentName,
            ApiDbContext<TContext> entities) 
            : base(componentIdentifier, componentName)
        {
            entities.NotNull(nameof(entities));

            _entities = entities;
        }

        /// <summary>
        /// Checks whether or not the db is up and running
        /// </summary>
        protected override Task CheckComponentResponsivenessAsync()
        {
            return Task.Run(() =>
            {
                _entities.Database.Connection.Open();
                _entities.Database.Connection.Close();
            });            
        }
    }
}
