﻿using System;
using System.Collections.Generic;
using GlmSharp;

namespace Globe3DLight.ViewModels.Scene
{
    public class AntennaRenderModel : BaseRenderModel
    {
        private dvec3 _absoluteTargetPostion;

        public dvec3 AbsoluteTargetPostion
        {
            get => _absoluteTargetPostion;
            set => RaiseAndSetIfChanged(ref _absoluteTargetPostion, value);
        }

        public override bool IsDirty()
        {
            var isDirty = base.IsDirty();

            return isDirty;
        }

        public override void Invalidate()
        {
            base.Invalidate();
        }
    }
}
