#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Monitoring.Core;
using C4rm4x.WebApi.Monitoring.Counter;
using C4rm4x.WebApi.Persistance.Mongo;
using System;
using System.Linq.Expressions;

#endregion

namespace C4rm4x.WebApi.Monitoring.Mongo
{
    /// <summary>
    /// Base implementation of a counter using an Mongo Repository
    /// </summary>
    /// <typeparam name="T">Type of the entity</typeparam>    
    public abstract class AbstractCounter<T> :
        AbstractMonitorService<long>,
        ICounter
        where T : BaseEntity
    {
        private readonly BaseRepository<T> _repository;
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
            BaseRepository<T> repository,
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
}
