using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight.Data.Database;

namespace Globe3DLight.Data.Animators
{
    public interface IFrameData : IData, IFrameable
    {   
      //  dmat4 ModelMatrix { get; }
    }


    public class FrameData : ObservableObject, IFrameData
    {
  
        private dmat4 _modelMatrix;

        public dmat4 ModelMatrix
        {
            get => _modelMatrix;
            protected set => Update(ref _modelMatrix, value);
        }

        public FrameData()
        {
            this._modelMatrix = dmat4.Identity;
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}
