using System;
using System.Collections;
using System.Collections.Generic;

namespace TimeDataViewer.Core
{
    public class ElementCollection<T> : IList<T> where T : Element
    {
        private readonly Model _parent;
        private readonly List<T> _internalList = new();

        public ElementCollection(Model parent)
        {
            _parent = parent;
        }

        public event EventHandler<ElementCollectionChangedEventArgs<T>> CollectionChanged;

        public int Count => _internalList.Count;

        public bool IsReadOnly => false;

        public T this[int index]
        {
            get
            {
                return _internalList[index];
            }

            set
            {
                value.Parent = _parent;
                _internalList[index] = value;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            if (item.Parent != null)
            {
                throw new InvalidOperationException("The element cannot be added, it already belongs to a PlotModel.");
            }

            item.Parent = _parent;
            _internalList.Add(item);

            RaiseCollectionChanged(new[] { item });
        }

        public void Clear()
        {
            var removedItems = new List<T>();

            foreach (var item in _internalList)
            {
                item.Parent = null;
                removedItems.Add(item);
            }

            _internalList.Clear();

            RaiseCollectionChanged(removedItems: removedItems);
        }

        public bool Contains(T item)
        {
            return _internalList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _internalList.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            item.Parent = null;
            var result = _internalList.Remove(item);
            if (result)
            {
                RaiseCollectionChanged(removedItems: new[] { item });
            }

            return result;
        }

        public int IndexOf(T item)
        {
            return _internalList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            if (item.Parent != null)
            {
                throw new InvalidOperationException("The element cannot be inserted, it already belongs to a PlotModel.");
            }

            item.Parent = _parent;
            _internalList.Insert(index, item);

            RaiseCollectionChanged(new[] { item });
        }

        public void RemoveAt(int index)
        {
            var item = this[index];
            item.Parent = null;

            _internalList.RemoveAt(index);

            RaiseCollectionChanged(removedItems: new[] { item });
        }

        private void RaiseCollectionChanged(IEnumerable<T> addedItems = null, IEnumerable<T> removedItems = null)
        {
            CollectionChanged?.Invoke(this, new ElementCollectionChangedEventArgs<T>(addedItems, removedItems));
        }
    }
}
