using System;
using System.Collections.Generic;
using GlmSharp;

namespace Globe3DLight.ViewModels.Scene
{
    public class AntennaRenderModel : BaseRenderModel
    {
        private dvec3 _targetPostion;
        private dvec3 _attachPosition;

        public dvec3 TargetPostion
        {
            get => _targetPostion;
            set => RaiseAndSetIfChanged(ref _targetPostion, value);
        }

        public dvec3 AttachPosition
        {
            get => _attachPosition;
            set => RaiseAndSetIfChanged(ref _attachPosition, value);
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
