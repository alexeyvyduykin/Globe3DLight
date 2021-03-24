using System;
using System.Collections.Generic;
using Globe3DLight.ViewModels.Data;

namespace Globe3DLight.ViewModels.Scene
{
    public class SensorRenderModel : BaseRenderModel
    {
        private Scan _scan;
        private Shoot _shoot;

        public Scan Scan
        {
            get => _scan;
            set => RaiseAndSetIfChanged(ref _scan, value);
        }

        public Shoot Shoot
        {
            get => _shoot;
            set => RaiseAndSetIfChanged(ref _shoot, value);
        }

        public override bool IsDirty()
        {
            var isDirty = base.IsDirty();

           // isDirty |= Scan.IsDirty();
           // isDirty |= Shoot.IsDirty();

            return isDirty;
        }

        public override void Invalidate()
        {
            base.Invalidate();
          //  Scan.Invalidate();
          //  Shoot.Invalidate();
        }
    }
}
