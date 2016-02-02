#region Using

using Autofac;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Events;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Framework.Autofac.Events
{
    /// <summary>
    /// Implementation of the Event Aggregator Design Pattern using an Autofac Container to retrieve all the event handlers
    /// </summary>
    public class EventAggregator
        : AbstractEventAggregator, IEventAggregator
    {
        private readonly IComponentContext _context;

        /// <summary>
        /// Gets the list of all the data pending to be published as a read-only collection
        /// </summary>
        public IReadOnlyCollection<ApiEventData> DataQueue
        {
            get { return Queue.ToList().AsReadOnly(); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Autofac context</param>
        public EventAggregator(IComponentContext context)
            : base()
        {
            context.NotNull(nameof(context));

            _context = context;
        }

        /// <summary>
        /// Retrieves the list of all (if any) event handlers that implement
        /// the interface IEventHandler fot the specified TEvent
        /// </summary>
        /// <typeparam name="TEvent">Type of event</typeparam>
        /// <returns>The list of all event hadlers for the specified type</returns>
        protected override IEnumerable<IEventHandler<TEvent>>
            GetHandlers<TEvent>()
        {
            return _context.ResolveAll<IEventHandler<TEvent>>()
                ?? new IEventHandler<TEvent>[] { };
        }
    }
}
