#nullable disable
using OpenTK.Graphics.OpenGL;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal enum ProgramPointSize
    {
        Enabled,
        Disabled
    }

    internal class RenderState
    {
        public PrimitiveRestart PrimitiveRestart { get; set; }

        public FacetCulling FacetCulling { get; set; }

        public ProgramPointSize ProgramPointSize { get; set; }

        public PolygonMode RasterizationMode { get; set; }

        public ScissorTest ScissorTest { get; set; }

        public StencilTest StencilTest { get; set; }

        public DepthTest DepthTest { get; set; }

        public DepthRange DepthRange { get; set; }

        public Blending Blending { get; set; }

        public ColorMask ColorMask { get; set; }

        public bool DepthMask { get; set; }
    }
}
