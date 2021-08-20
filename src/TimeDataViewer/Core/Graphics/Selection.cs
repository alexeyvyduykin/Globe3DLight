using System;
using System.Collections.Generic;
using System.Linq;

namespace TimeDataViewer.Core
{
    public class Selection
    {
        /// <summary>
        /// Static instance representing everything (all items and all features) selected.
        /// </summary>
        private static readonly Selection EverythingSelection = new();

        /// <summary>
        /// The selection (cannot use HashSet{T} in PCL)
        /// </summary>
        private readonly Dictionary<SelectionItem, bool> _selection = new();

        /// <summary>
        /// Gets the everything selected.
        /// </summary>
        /// <value>The everything.</value>
        public static Selection Everything => EverythingSelection;

        /// <summary>
        /// Determines whether everything is selected.
        /// </summary>
        /// <returns><c>true</c> if everything is selected; otherwise, <c>false</c>.</returns>
        public bool IsEverythingSelected()
        {
            // ReSharper disable RedundantNameQualifier
            return object.ReferenceEquals(this, EverythingSelection);
            // ReSharper restore RedundantNameQualifier
        }

        /// <summary>
        /// Gets the indices of the selected items in this selection.
        /// </summary>
        /// <returns>Enumerator of indices.</returns>
        public IEnumerable<int> GetSelectedItems()
        {
            return _selection.Keys.Select(si => si.Index);
        }

        /// <summary>
        /// Gets the selected items by the specified feature.
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <returns>Enumerator of indices.</returns>
        public IEnumerable<int> GetSelectedItems(Enum feature)
        {
            // ReSharper disable RedundantNameQualifier
            return _selection.Keys.Where(si => object.Equals(si.Feature, feature)).Select(si => si.Index);
            // ReSharper restore RedundantNameQualifier
        }

        public void Clear()
        {
            _selection.Clear();
        }

        /// <summary>
        /// Determines whether the specified item and feature is selected.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <param name="feature">The feature.</param>
        /// <returns><c>true</c> if the item is selected; otherwise, <c>false</c>.</returns>
        public bool IsItemSelected(int index, Enum feature = null)
        {
            if (IsEverythingSelected())
            {
                return true;
            }

            var si = new SelectionItem(index, feature);
            return _selection.ContainsKey(si);
        }

        /// <summary>
        /// Selects the specified item/feature.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="feature">The feature.</param>
        public void Select(int index, Enum feature = null)
        {
            var si = new SelectionItem(index, feature);
            _selection[si] = true;
        }

        /// <summary>
        /// Unselects the specified item.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <param name="feature">The feature.</param>
        public void Unselect(int index, Enum feature = null)
        {
            var si = new SelectionItem(index, feature);
            if (!_selection.ContainsKey(si))
            {
                throw new InvalidOperationException("Item " + index + " and feature " + feature + " is not selected. Cannot unselect.");
            }

            _selection.Remove(si);
        }

        public struct SelectionItem : IEquatable<SelectionItem>
        {
            private readonly int _index;

            private readonly Enum _feature;

            public SelectionItem(int index, Enum feature)
            {
                _index = index;
                _feature = feature;
            }

            public int Index => _index;

            public Enum Feature => _feature;

            public bool Equals(SelectionItem other)
            {
                // ReSharper disable RedundantNameQualifier
                return other._index == _index && object.Equals(other._feature, _feature);
                // ReSharper restore RedundantNameQualifier
            }

            public override int GetHashCode()
            {
                if (_feature == null)
                {
                    return _index.GetHashCode();
                }

                // http://msdn.microsoft.com/en-us/library/system.object.gethashcode.aspx
                // http://stackoverflow.com/questions/2890040/implementing-gethashcode
                // http://stackoverflow.com/questions/508126/what-is-the-correct-implementation-for-gethashcode-for-entity-classes
                // http://stackoverflow.com/questions/70303/how-do-you-implement-gethashcode-for-structure-with-two-string
                return _index.GetHashCode() ^ _feature.GetHashCode();
            }
        }
    }
}
