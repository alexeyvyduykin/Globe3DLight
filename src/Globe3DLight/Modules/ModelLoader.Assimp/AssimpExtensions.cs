using GlmSharp;
using Assimp;

namespace Globe3DLight.ModelLoader.Assimp
{
    internal static class AssimpExtensions
    {
        public static vec4 ToVec4(this Color4D color)
        {
            return new vec4(color.R, color.G, color.B, color.A);
        }
    }
}
