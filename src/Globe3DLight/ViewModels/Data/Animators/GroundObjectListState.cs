using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;
using Globe3DLight.Models.Data;

namespace Globe3DLight.ViewModels.Data
{
    public class GroundObjectListState : ViewModelBase, IState
    {
        private IDictionary<string, GroundObjectState> _states;

        public GroundObjectListState(IDictionary<string, GroundObjectState> states)
        {
            _states = states;
        }

        public IDictionary<string, GroundObjectState> States
        {
            get => _states;
            protected set => RaiseAndSetIfChanged(ref _states, value);
        }
    }
}
