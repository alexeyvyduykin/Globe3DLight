using System;
using System.Collections.Generic;
using GlmSharp;

namespace Globe3DLight.Scene
{
    public class AntennaRenderModel : BaseRenderModel, IAntennaRenderModel
    {
        private dvec3 _targetPostion;
        private dvec3 _attachPosition;

        public dvec3 TargetPostion
        {
            get => _targetPostion;
            set => Update(ref _targetPostion, value);
        }

        public dvec3 AttachPosition
        {
            get => _attachPosition;
            set => Update(ref _attachPosition, value);
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

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}
