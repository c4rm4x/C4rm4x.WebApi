#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Monitoring.Core;
using C4rm4x.WebApi.Monitoring.Counter;
using C4rm4x.WebApi.Persistance.EF;
using System;
using System.Data.Entity;
using System.Linq.Expressions;

#endregion

namespace C4rm4x.WebApi.Monitoring.EF
{
    /// <summary>
    /// Base implementation of a counter using an EF Repository
    /// </summary>
    /// <typeparam name="T">Type of the entity</typeparam>
    /// <typeparam name="K">Type of the id</typeparam>
    /// <typeparam name="C">Type of db context</typeparam>
    public abstract class AbstractCounter<T, K, C> : 
        AbstractMonitorService<long>, 
        ICounter
        where T : class
        where C : DbContext
    {
        private readonly BaseRepository<T, K, C> _repository;
        private readonly Expression<Func<T, bool>> _predicate;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="componentIdentifier">Component's identifier</param>
        /// <param name="componentName">Component's name</param>
        /// <param name="repository">The repository responsible to count</param>
        /// <param name="predicate">The predicate</param>
        public AbstractCounter(
            object componentIdentifier, 
            string componentName,
            BaseRepository<T, K, C> repository,
            Expression<Func<T, bool>> predicate = null)
            : base(componentIdentifier, componentName)
        {
            repository.NotNull(nameof(repository));

            _repository = repository;
            _predicate = predicate;
        }

        /// <summary>
        /// Counts the number of all entities of type T (that fulfill the predicate if any)
        /// </summary>
        /// <returns>The total number of entities of type T (that fulfill the predicate if any)</returns>
        public override long Monitor()
        {
            return _predicate.IsNull()
                ? _repository.Count()
                : _repository.Count(_predicate);
        }
    }

    /// <summary>
    /// Base implementation of a counter using an EF Repository
    /// </summary>
    public abstract class AbstractCounter<T, C> :
        AbstractCounter<T, int, C>
        where T : class
        where C : DbContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="componentIdentifier">Component's identifier</param>
        /// <param name="componentName">Component's name</param>
        /// <param name="repository">The repository responsible to count</param>
        /// <param name="predicate">The predicate</param>
        public AbstractCounter(
            object componentIdentifier,
            string componentName,
            BaseRepository<T, C> repository,
            Expression<Func<T, bool>> predicate = null)
            : base(componentIdentifier, componentName, repository, predicate)
        { }        
    }    
}
