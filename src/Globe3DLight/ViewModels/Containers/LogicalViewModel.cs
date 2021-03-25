using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Immutable;
using Globe3DLight.ViewModels.Scene;
using Globe3DLight.ViewModels.Data;

namespace Globe3DLight.ViewModels.Containers
{
    public class LogicalViewModel : BaseContainerViewModel
    {
        private ImmutableArray<ViewModelBase> _children;       
        private BaseState _state;

        public ImmutableArray<ViewModelBase> Children
        {
            get => _children;
            set => RaiseAndSetIfChanged(ref _children, value);
        }

        public BaseState State
        {
            get => _state;
            set => RaiseAndSetIfChanged(ref _state, value);
        }

        public override bool IsDirty()
        {
            var isDirty = base.IsDirty();

            //if (State != null)
            //{
            //    isDirty |= State.IsDirty();
            //}

            foreach (var child in Children)
            {
                isDirty |= child.IsDirty();
            }

            return isDirty;
        }

        public override void Invalidate()
        {
            base.Invalidate();

            //State?.Invalidate();

            foreach (var child in Children)
            {
                child.Invalidate();
            }
        }
    }
}
