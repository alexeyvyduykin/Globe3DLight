using System;
using System.Collections.Immutable;
using System.Text;

namespace Globe3DLight.Containers
{
    public interface IProjectContainer : IBaseContainer
    {
        ImmutableArray<IScenarioContainer> Scenarios { get; set; }

        IScenarioContainer CurrentScenario { get; set; }

        IObservableObject Selected { get; set; }

        void SetCurrentScenario(IScenarioContainer scenario);

        void SetCurrentTemplate(IScenarioContainer template);

        void SetSelected(IObservableObject value);
    }
}
