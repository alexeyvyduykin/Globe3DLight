using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.ViewModels.Containers
{
    public class BaseContainerViewModel : ViewModelBase
    {
        private bool _isVisible;
        private bool _isExpanded;

        protected BaseContainerViewModel()
        {

        }

        public bool IsVisible
        {
            get => _isVisible;
            set => RaiseAndSetIfChanged(ref _isVisible, value);
        }

        public bool IsExpanded
        {
            get => _isExpanded;
            set => RaiseAndSetIfChanged(ref _isExpanded, value);
        }
    }
}
