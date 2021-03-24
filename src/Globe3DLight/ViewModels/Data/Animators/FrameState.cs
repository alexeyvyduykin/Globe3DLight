using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Models.Data;
using GlmSharp;
using Globe3DLight.Models;

namespace Globe3DLight.ViewModels.Data
{
    public class FrameState : ViewModelBase, IState, IFrameable
    {  
        private dmat4 _modelMatrix;

        public dmat4 ModelMatrix
        {
            get => _modelMatrix;
            protected set => RaiseAndSetIfChanged(ref _modelMatrix, value);
        }

        public FrameState()
        {
            _modelMatrix = dmat4.Identity;
        }
    }
}
