using System;
using System.Collections.Generic;
using GlmSharp;

namespace Globe3DLight.ViewModels.Scene
{
    public class OrbitRenderModel : BaseRenderModel
    {
        private IList<dvec3> _vertices;

        public IList<dvec3> Vertices
        {
            get => _vertices;
            set => RaiseAndSetIfChanged(ref _vertices, value);
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
