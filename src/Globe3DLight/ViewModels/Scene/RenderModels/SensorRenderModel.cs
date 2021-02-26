using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight;
using Globe3DLight.Data;
using Globe3DLight.Renderer;
using Globe3DLight.Data.Animators;


namespace Globe3DLight.Scene
{



    public class SensorRenderModel : BaseRenderModel, ISensorRenderModel
    {
        private IScan _scan;
        private IShoot _shoot;

        public IScan Scan 
        { 
            get => _scan; 
            set => Update(ref _scan, value); 
        }

        public IShoot Shoot 
        {
            get => _shoot;
            set => Update(ref _shoot, value); 
        }

        public override bool IsDirty()
        {
            var isDirty = base.IsDirty();

            isDirty |= Scan.IsDirty();
            isDirty |= Shoot.IsDirty();

            return isDirty;
        }

        /// <inheritdoc/>
        public override void Invalidate()
        {
            base.Invalidate();
            Scan.Invalidate();
            Shoot.Invalidate();
        }


        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }

    }


}
