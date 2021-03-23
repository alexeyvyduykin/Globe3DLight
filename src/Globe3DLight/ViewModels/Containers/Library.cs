using System;
using System.Collections.Immutable;
using System.Text;
using System.Collections.Generic;


namespace Globe3DLight.Containers
{
    public class Library<T> : ObservableObject//, ILibrary<T>
    {
        private ImmutableArray<T> _items;
        private T _selected;
      
        public ImmutableArray<T> Items
        {
            get => _items;
            set => Update(ref _items, value);
        }
  
        public T Selected
        {
            get => _selected;
            set => Update(ref _selected, value);
        }
        
        public void SetSelected(T item) => Selected = item;
       
        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
 
        public override bool IsDirty()
        {
            var isDirty = base.IsDirty();

            foreach (var item in Items)
            {
                if (item is ObservableObject observableObject)
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
                if (item is ObservableObject observableObject)
                {
                    observableObject.Invalidate();
                }
            }
        }
    }
}
