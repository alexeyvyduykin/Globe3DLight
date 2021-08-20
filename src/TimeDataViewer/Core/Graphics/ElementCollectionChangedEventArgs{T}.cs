using System;
using System.Collections.Generic;

namespace TimeDataViewer.Core
{
    public class ElementCollectionChangedEventArgs<T> : EventArgs
    {
        public ElementCollectionChangedEventArgs(IEnumerable<T> addedItems, IEnumerable<T> removedItems)
        {
            AddedItems = new List<T>(addedItems ?? new T[] { });
            RemovedItems = new List<T>(removedItems ?? new T[] { });
        }

        public List<T> AddedItems { get; private set; }

        public List<T> RemovedItems { get; private set; }
    }
}
