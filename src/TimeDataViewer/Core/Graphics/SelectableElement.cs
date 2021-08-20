using System;
using System.Collections.Generic;

namespace TimeDataViewer.Core
{
    public abstract class SelectableElement : Element
    {
        private Selection? _selection;

        protected SelectableElement()
        {
            Selectable = true;
            SelectionMode = SelectionMode.All;
        }

        /// <summary>
        /// Occurs when the selected items is changed.
        /// </summary>
        public event EventHandler? SelectionChanged;

        /// <summary>
        /// Gets or sets a value indicating whether this element can be selected. The default is <c>true</c>.
        /// </summary>
        public bool Selectable { get; set; }

        /// <summary>
        /// Gets or sets the selection mode of items in this element. The default is <c>SelectionMode.All</c>.
        /// </summary>
        /// <value>The selection mode.</value>
        /// <remarks>This is only used by the select/unselect functionality, not by the rendering.</remarks>
        public SelectionMode SelectionMode { get; set; }

        /// <summary>
        /// Determines whether any part of this element is selected.
        /// </summary>
        /// <returns><c>true</c> if this element is selected; otherwise, <c>false</c>.</returns>
        public bool IsSelected()
        {
            return _selection != null;
        }

        /// <summary>
        /// Gets the indices of the selected items in this element.
        /// </summary>
        /// <returns>Enumerator of item indices.</returns>
        public IEnumerable<int> GetSelectedItems()
        {
            EnsureSelection();
            return _selection.GetSelectedItems();
        }

        public void ClearSelection()
        {
            _selection = null;
            OnSelectionChanged();
        }

        public void Unselect()
        {
            _selection = null;
            OnSelectionChanged();
        }

        /// <summary>
        /// Determines whether the specified item is selected.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <returns><c>true</c> if the item is selected; otherwise, <c>false</c>.</returns>
        public bool IsItemSelected(int index)
        {
            if (_selection == null)
            {
                return false;
            }

            if (index == -1)
            {
                return _selection.IsEverythingSelected();
            }

            return _selection.IsItemSelected(index);
        }

        /// <summary>
        /// Selects all items in this element.
        /// </summary>
        public void Select()
        {
            _selection = Selection.Everything;
            OnSelectionChanged();
        }

        public void SelectItem(int index)
        {
            if (SelectionMode == SelectionMode.All)
            {
                throw new InvalidOperationException("Use the Select() method when using SelectionMode.All");
            }

            EnsureSelection();
            if (SelectionMode == SelectionMode.Single)
            {
                _selection.Clear();
            }

            _selection.Select(index);
            OnSelectionChanged();
        }

        public void UnselectItem(int index)
        {
            if (SelectionMode == SelectionMode.All)
            {
                throw new InvalidOperationException("Use the Unselect() method when using SelectionMode.All");
            }

            EnsureSelection();
            _selection.Unselect(index);
            OnSelectionChanged();
        }

        private void EnsureSelection()
        {
            if (_selection == null)
            {
                _selection = new Selection();
            }
        }

        private void OnSelectionChanged(EventArgs args = null)
        {
            SelectionChanged?.Invoke(this, args);
        }
    }
}
