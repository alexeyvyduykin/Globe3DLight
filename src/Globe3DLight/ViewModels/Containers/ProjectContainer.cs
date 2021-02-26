using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Globe3DLight.Containers
{
    public partial class ProjectContainer : ObservableObject, IProjectContainer
    {
        private ImmutableArray<IScenarioContainer> _scenarios;
        private IScenarioContainer _currentScenario;
        private IObservableObject _selected;

        public ImmutableArray<IScenarioContainer> Scenarios 
        { 
            get => _scenarios; 
            set => Update(ref _scenarios, value); 
        }

        public IScenarioContainer CurrentScenario
        {
            get => _currentScenario; 
            set => Update(ref _currentScenario, value); 
        
        }

        public IObservableObject Selected 
        {
            get => _selected;
            set 
            {                 
                SetSelected(value);
                Update(ref _selected, value);
            }
        }

        public void SetCurrentScenario(IScenarioContainer scenario)
        {
            CurrentScenario = scenario;
            Selected = scenario;
        }

        public void SetCurrentTemplate(IScenarioContainer template)
        {
            throw new NotImplementedException();
        }

        public void SetSelected(IObservableObject value)
        {
            if(value is IScenarioContainer scenario)
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
