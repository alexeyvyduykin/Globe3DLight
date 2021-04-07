using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Models.Data;
using GlmSharp;
using Globe3DLight.Models;

namespace Globe3DLight.ViewModels.Data
{
    public class IdentityState : BaseState, IFrameable
    {  
        public IdentityState()
        {
            ModelMatrix = dmat4.Identity;
        }
    }
}
