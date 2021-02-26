using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal enum ProgramPointSize
    {
        Enabled,
        Disabled
    }

    //public enum RasterizationMode
    //{
    //    Point,
    //    Line,
    //    Fill
    //}

    internal class RenderState
    {
        public RenderState()
        {
            PrimitiveRestart = new PrimitiveRestart();
            FacetCulling = new FacetCulling();
            ProgramPointSize = ProgramPointSize.Disabled;
            RasterizationMode = PolygonMode.Fill;// RasterizationMode.Fill;
            ScissorTest = new ScissorTest();
            StencilTest = new StencilTest();
            DepthTest = new DepthTest();
            DepthRange = new DepthRange();
            Blending = new Blending();
            ColorMask = new ColorMask(true, true, true, true);
            DepthMask = true;
        }

        public PrimitiveRestart PrimitiveRestart { get; set; }
        public FacetCulling FacetCulling { get; set; }
        public ProgramPointSize ProgramPointSize { get; set; }
        public /*RasterizationMode*/ PolygonMode RasterizationMode { get; set; }
        public ScissorTest ScissorTest { get; set; }
        public StencilTest StencilTest { get; set; }
        public DepthTest DepthTest { get; set; }
        public DepthRange DepthRange { get; set; }
        public Blending Blending { get; set; }
        public ColorMask ColorMask { get; set; }
        public bool DepthMask { get; set; }
    }
}
