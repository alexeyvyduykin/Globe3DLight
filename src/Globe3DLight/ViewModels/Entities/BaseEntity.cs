#nullable disable
using System.Collections.Immutable;
using Globe3DLight.ViewModels.Containers;

namespace Globe3DLight.ViewModels.Entities
{
    public abstract class BaseEntity : BaseContainerViewModel
    {
        private ImmutableArray<BaseEntity> _children;
        //private LogicalCollectionViewModel _logicalCollection;

        public ImmutableArray<BaseEntity> Children
        {
            get => _children;
            set => RaiseAndSetIfChanged(ref _children, value);
        }

        //public LogicalCollectionViewModel LogicalCollection
        //{
        //    get => _logicalCollection;
        //    set => RaiseAndSetIfChanged(ref _logicalCollection, value);
        //}

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
