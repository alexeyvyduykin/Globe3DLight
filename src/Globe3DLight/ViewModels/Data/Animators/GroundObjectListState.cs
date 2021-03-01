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
        IDictionary<string, IGroundObjectState> States { get; }
    }


    public class GroundObjectListState : ObservableObject, IGroundObjectListState
    {
        private IDictionary<string, IGroundObjectState> _states;

        public GroundObjectListState(IDictionary<string, IGroundObjectState> states)
        {
            _states = states;
        }

        public IDictionary<string, IGroundObjectState> States
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
