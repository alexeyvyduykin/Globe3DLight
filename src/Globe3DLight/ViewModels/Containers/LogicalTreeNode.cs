using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Immutable;
using Globe3DLight.Scene;
using Globe3DLight.Data;


namespace Globe3DLight.Containers
{
    public class Logical : ObservableObject, ILogical
    {
        private ImmutableArray<IObservableObject> _children;
        private bool _isExpanded = true;
        private IState _state;

        public bool IsExpanded
        {
            get => _isExpanded;
            set => Update(ref _isExpanded, value);
        }

        public ImmutableArray<IObservableObject> Children
        {
            get => _children;
            set => Update(ref _children, value);
        }

        public IState State
        {
            get => _state;
            set => Update(ref _state, value);
        }

        public override bool IsDirty()
        {
            var isDirty = base.IsDirty();

            if (State != null)
            {
                isDirty |= State.IsDirty();
            }

            foreach (var child in Children)
            {
                isDirty |= child.IsDirty();
            }

            return isDirty;
        }

        public override void Invalidate()
        {
            base.Invalidate();

            State?.Invalidate();

            foreach (var child in Children)
            {
                child.Invalidate();
            }
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }

    public class LogicalCollection : ObservableObject, ILogicalCollection
    {
        private ImmutableArray<ILogical> _values;
        private bool _isExpanded = true;
        private IEnumerable<IState> _state;

        public bool IsExpanded
        {
            get => _isExpanded;
            set => Update(ref _isExpanded, value);
        }

        public IEnumerable<IState> State
        {
            get => _state;
            set => Update(ref _state, value);
        }
        public ImmutableArray<ILogical> Values
        {
            get => _values;
            set => Update(ref _values, value);
        }

        public override bool IsDirty()
        {
            var isDirty = base.IsDirty();

            foreach (var st in State)
            {
                isDirty |= st.IsDirty();
            }

            return isDirty;
        }

        public override void Invalidate()
        {
            base.Invalidate();
      
            foreach (var st in State)
            {
                st.Invalidate();
            }
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}
