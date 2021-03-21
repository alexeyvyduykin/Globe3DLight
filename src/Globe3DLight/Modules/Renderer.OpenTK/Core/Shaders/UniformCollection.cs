using System.Collections.ObjectModel;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal class UniformCollection : KeyedCollection<string, Uniform>
    {
        protected override string GetKeyForItem(Uniform item)
        {
            return item.Name;
        }
    }
}
