using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Immutable;
using Globe3DLight.Scene;
using Globe3DLight.Data;


namespace Globe3DLight.Containers
{
    public class LogicalTreeNode : ObservableObject, ILogicalTreeNode
    {       
        private ImmutableArray<ILogicalTreeNode> _children;    
        private bool _isExpanded = true;
        private IState _state;

        public bool IsExpanded
        {
            get => _isExpanded;
            set => Update(ref _isExpanded, value);
        }

        //public new string Name
        //{
        //    get => (this.Data != null) ? this.Data.Name : string.Empty;
        //    set
        //    {
        //        if (this.Data != null)
        //        {
        //            this.Data.Name = value;
        //        }
        //    }
        //}


        public ImmutableArray<ILogicalTreeNode> Children
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

        /// <inheritdoc/>
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
}
