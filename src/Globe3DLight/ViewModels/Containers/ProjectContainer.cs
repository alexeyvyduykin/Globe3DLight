using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Globe3DLight.Containers
{
    public partial class ProjectContainer : ObservableObject
    {
        private ImmutableArray<ScenarioContainer> _scenarios;
        private ScenarioContainer _currentScenario;
        private ObservableObject _selected;

        public ImmutableArray<ScenarioContainer> Scenarios 
        { 
            get => _scenarios; 
            set => Update(ref _scenarios, value); 
        }

        public ScenarioContainer CurrentScenario
        {
            get => _currentScenario; 
            set => Update(ref _currentScenario, value); 
        
        }

        public ObservableObject Selected 
        {
            get => _selected;
            set 
            {                 
                SetSelected(value);
                Update(ref _selected, value);
            }
        }

        public void SetCurrentScenario(ScenarioContainer scenario)
        {
            CurrentScenario = scenario;
            Selected = scenario;
        }

        public void SetCurrentTemplate(ScenarioContainer template)
        {
            throw new NotImplementedException();
        }

        public void SetSelected(ObservableObject value)
        {
            if(value is ScenarioContainer scenario)
            {
                this.CurrentScenario = scenario;
            }
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }


        public virtual bool ShouldSerializeScenarios() => true;
        public virtual bool ShouldSerializeCurrentScenario() => _currentScenario != null;
        public virtual bool ShouldSerializeSelected() => _selected != null;
    }
}
