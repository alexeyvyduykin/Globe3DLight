using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Immutable;

namespace Globe3DLight.ScenarioObjects
{
    public abstract class BaseScenarioObject : ObservableObject, IScenarioObject
    {
        private ImmutableArray<IScenarioObject> _children;
        private bool _isVisible;
        private bool _isExpanded;

        public ImmutableArray<IScenarioObject> Children
        {
            get => _children;
            set => Update(ref _children, value);
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

            foreach (var child in Children)
            {
                isDirty |= child.IsDirty();
            }

            //   isDirty |= State.IsDirty();
            //   isDirty |= Data.IsDirty();

            return isDirty;
        }

        public override void Invalidate()
        {
            base.Invalidate();

            foreach (var child in Children)
            {
                child.Invalidate();
            }

            //    State.Invalidate();
            //    Data.Invalidate();
        }
    }
}
