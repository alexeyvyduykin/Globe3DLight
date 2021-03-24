using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Immutable;

namespace Globe3DLight.ViewModels.Containers
{

    public class LogicalCollectionViewModel : BaseContainerViewModel
    {
        private ImmutableArray<LogicalViewModel> _values;
        private IEnumerable<ViewModelBase> _state;

        public IEnumerable<ViewModelBase> State
        {
            get => _state;
            set => RaiseAndSetIfChanged(ref _state, value);
        }

        public ImmutableArray<LogicalViewModel> Values
        {
            get => _values;
            set => RaiseAndSetIfChanged(ref _values, value);
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
    }
}
