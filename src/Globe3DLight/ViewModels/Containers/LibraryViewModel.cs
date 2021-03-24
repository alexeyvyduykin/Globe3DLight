using System;
using System.Collections.Immutable;
using System.Text;
using System.Collections.Generic;

namespace Globe3DLight.ViewModels.Containers
{
    public class LibraryViewModel<T> : ViewModelBase
    {
        private ImmutableArray<T> _items;
        private T _selected;
      
        public ImmutableArray<T> Items
        {
            get => _items;
            set => RaiseAndSetIfChanged(ref _items, value);
        }
  
        public T Selected
        {
            get => _selected;
            set => RaiseAndSetIfChanged(ref _selected, value);
        }
        
        public void SetSelected(T item) => Selected = item;
        
        public override bool IsDirty()
        {
            var isDirty = base.IsDirty();

            foreach (var item in Items)
            {
                if (item is ViewModelBase observableObject)
                {
                    isDirty |= observableObject.IsDirty();
                }
            }

            return isDirty;
        }

        public override void Invalidate()
        {
            base.Invalidate();

            foreach (var item in Items)
            {
                if (item is ViewModelBase observableObject)
                {
                    observableObject.Invalidate();
                }
            }
        }
    }
}
