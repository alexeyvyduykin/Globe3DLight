using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Immutable;
using Globe3DLight.Containers;

namespace Globe3DLight.Entities
{
    public abstract class BaseEntity : ObservableObject, IEntity
    {
        private ImmutableArray<IEntity> _children;
        private ILogicalCollection _logicalCollection;
        private bool _isVisible;
        private bool _isExpanded;

        public ImmutableArray<IEntity> Children
        {
            get => _children;
            set => Update(ref _children, value);
        }
       

        public ILogicalCollection LogicalCollection
        {
            get => _logicalCollection;
            set => Update(ref _logicalCollection, value);
        }

        public bool IsVisible
        {
            get => _isVisible;
            set => Update(ref _isVisible, value);
        }

        public bool IsExpanded 
        {
            get => _isExpanded; 
            set => Update(ref _isExpanded, value); 
        }

        public override bool IsDirty()
        {
            var isDirty = base.IsDirty();

            if (Children != null)
            {
                foreach (var child in Children)
                {
                    isDirty |= child.IsDirty();
                }
            }
            //   isDirty |= State.IsDirty();
            //   isDirty |= Data.IsDirty();

            return isDirty;
        }

        public override void Invalidate()
        {
            base.Invalidate();

            if (Children != null)
            {
                foreach (var child in Children)
                {
                    child.Invalidate();
                }
            }
            //    State.Invalidate();
            //    Data.Invalidate();
        }
    }
}
