#region Using

using System;
using System.Collections;
using System.Collections.Generic;

#endregion

namespace C4rm4x.WebApi.Validation.Core
{
    internal class TrackingCollection<T> : IEnumerable<T>
    {
        private readonly List<T> _innerCollection = new List<T>();

        public event Action<T> ItemAdded;

        public void Add(T item)
        {
            _innerCollection.Add(item);

            OnItemAdded(item);
        }

        private void OnItemAdded(T itemAdded)
        {
            if (this.ItemAdded != null)
                this.ItemAdded(itemAdded);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _innerCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IDisposable OnItemAdded(Action<T> handler)
        {
            this.ItemAdded += handler;

            return new DisposableEvent(this, handler);
        }

        class DisposableEvent : IDisposable
        {
            private readonly TrackingCollection<T> _collection;
            private readonly Action<T> _handler;

            public DisposableEvent(TrackingCollection<T> collection, Action<T> handler)
            {
                _collection = collection;
                _handler = handler;
            }

            public void Dispose()
            {
                _collection.ItemAdded -= _handler;
            }
        }
    }
}
