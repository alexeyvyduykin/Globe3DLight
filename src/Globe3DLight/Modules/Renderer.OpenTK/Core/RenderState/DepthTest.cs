using OpenTK.Graphics.OpenGL;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal class DepthTest
    {
        public DepthTest()
        {
            Enabled = true;
            Function = DepthFunction.Less;
        }

        public bool Enabled { get; set; }
        public DepthFunction Function { get; set; }
    }
}
