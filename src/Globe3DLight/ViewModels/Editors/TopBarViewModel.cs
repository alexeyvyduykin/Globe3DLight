using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.ViewModels.Editors
{
    public enum Workspace { Layout, Planning };

    public class TopBarViewModel : ViewModelBase
    {
        private Workspace _activeWorkspace;
        
        public TopBarViewModel()
        {
            PropertyChanged += TopBarViewModel_PropertyChanged;
        }

        private void TopBarViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(TopBarViewModel.ActiveWorkspace))
            {
            //    RaisePropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(TopBarViewModel)));        
            }
        }

        public Workspace ActiveWorkspace
        {
            get => _activeWorkspace;
            set => RaiseAndSetIfChanged(ref _activeWorkspace, value);
        }
    }
}
