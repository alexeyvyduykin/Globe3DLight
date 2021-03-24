using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Globe3DLight.ViewModels.Containers
{
    public partial class ProjectContainerViewModel : BaseContainerViewModel
    {
        private ImmutableArray<ScenarioContainerViewModel> _scenarios;
        private ScenarioContainerViewModel _currentScenario;
        private ViewModelBase _selected;

        public ImmutableArray<ScenarioContainerViewModel> Scenarios 
        { 
            get => _scenarios; 
            set => RaiseAndSetIfChanged(ref _scenarios, value); 
        }

        public ScenarioContainerViewModel CurrentScenario
        {
            get => _currentScenario; 
            set => RaiseAndSetIfChanged(ref _currentScenario, value);         
        }

        public ViewModelBase Selected
        {
            get => _selected;
            set 
            {                 
                SetSelected(value);
                RaiseAndSetIfChanged(ref _selected, value);
            }
        }

        public void SetCurrentScenario(ScenarioContainerViewModel scenario)
        {
            CurrentScenario = scenario;
            Selected = scenario;
        }

        public void SetSelected(ViewModelBase value)
        {
            if(value is ScenarioContainerViewModel scenario)
            {
                CurrentScenario = scenario;
            }
        }
    }
}
