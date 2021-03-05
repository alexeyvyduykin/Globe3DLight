using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;
using Globe3DLight;
using Globe3DLight.Data;
using Globe3DLight.Renderer;

namespace Globe3DLight.Scene
{

    public class OrbitRenderModel : BaseRenderModel, IOrbitRenderModel
    {
        private IList<dvec3> _vertices;

        public IList<dvec3> Vertices 
        {
            get => _vertices; 
            set => Update(ref _vertices, value); 
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
