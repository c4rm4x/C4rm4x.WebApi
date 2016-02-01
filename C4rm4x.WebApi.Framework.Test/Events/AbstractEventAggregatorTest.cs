#region Using

using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using C4rm4x.WebApi.Framework.Events;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Events
{
    public partial class AbstractEventAggregatorTest
    {
        #region Helper classes

        class TestEventAggregator : AbstractEventAggregator
        {
            private readonly IDictionary<Type, IEnumerable> _handlers;

            public TestEventAggregator()
            {
                _handlers = new Dictionary<Type, IEnumerable>();
            }

            public IEnumerable<ApiEventData> DataQueue
            {
                get { return Queue.ToList().AsReadOnly(); }
            }

            protected override IEnumerable<IEventHandler<TEvent>> GetHandlers<TEvent>()
            {
                return _handlers.ContainsKey(typeof(TEvent))
                    ? _handlers[typeof(TEvent)].OfType<IEventHandler<TEvent>>()
                    : new IEventHandler<TEvent>[] { };
            }

            /// <summary>
            /// Adds handlers and overwrites previous ones (if any)
            /// </summary>
            /// <typeparam name="TEvent">Type of event</typeparam>
            /// <param name="handlers">Event handlers</param>
            public void AddHandlers<TEvent>(IEnumerable<IEventHandler<TEvent>> handlers)
                where TEvent : ApiEventData
            {
                _handlers.Add(typeof(TEvent), handlers);
            }
        }

        public class TestEventData : ApiEventData { }

        #endregion

        private static TestEventAggregator CreateSubjectUnderTest()
        {
            return new TestEventAggregator();
        }

        private static TestEventAggregator CreateSubjectUnderTest<TEvent>(
            params IEventHandler<TEvent>[] handlers)
            where TEvent : ApiEventData
        {
            var eventAggregator = CreateSubjectUnderTest();

            eventAggregator.AddHandlers(handlers);

            return eventAggregator;
        }

        private static int GetRand(int max)
        {
            return new Random().Next(1, max);
        }

        private static IEnumerable<IEventHandler<TEvent>>
            GetHandlers<TEvent>(int numberOfHandlers)
            where TEvent : ApiEventData
        {
            for (int i = 0; i < numberOfHandlers; i++)
                yield return Mock.Of<IEventHandler<TEvent>>();
        }
    }
}
