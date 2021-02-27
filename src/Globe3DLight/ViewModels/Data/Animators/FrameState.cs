using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;

namespace Globe3DLight.Data
{
    public interface IFrameState : IState, IFrameable
    {   
      //  dmat4 ModelMatrix { get; }
    }


    public class FrameState : ObservableObject, IFrameState
    {
  
        private dmat4 _modelMatrix;

        public dmat4 ModelMatrix
        {
            get => _modelMatrix;
            protected set => Update(ref _modelMatrix, value);
        }

        public FrameState()
        {
            this._modelMatrix = dmat4.Identity;
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}
