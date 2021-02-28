using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;

namespace Globe3DLight.Data
{
    public interface IGroundObjectListState : IState
    {
        IDictionary<string, GroundObjectState> States { get; }
    }


    public class GroundObjectListState : ObservableObject, IGroundObjectListState
    {
        private IDictionary<string, GroundObjectState> _states;

        public GroundObjectListState(IDictionary<string, GroundObjectState> states)
        {
            _states = states;
        }

        public IDictionary<string, GroundObjectState> States
        {
            get => _states;
            protected set => Update(ref _states, value);
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}
