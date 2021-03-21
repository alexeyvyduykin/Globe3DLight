using System.Collections.ObjectModel;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal class ShaderVertexAttributeCollection : KeyedCollection<string, ShaderVertexAttribute>
    {
        protected override string GetKeyForItem(ShaderVertexAttribute item)
        {
            return item.Name;
        }
    }
}
