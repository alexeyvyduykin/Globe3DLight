using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Models.Data;
using GlmSharp;
using Globe3DLight.Models;

namespace Globe3DLight.ViewModels.Data
{
    public class FrameState : BaseState, IFrameable
    {  
        public FrameState()
        {
            ModelMatrix = dmat4.Identity;
        }
    }
}
