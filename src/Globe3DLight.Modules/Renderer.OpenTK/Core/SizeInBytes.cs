using System.Runtime.InteropServices;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal static class SizeInBytes<T>
    {
        public static readonly int Value = Marshal.SizeOf(typeof(T));
    }
}
