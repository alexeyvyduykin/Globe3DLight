using System.Runtime.InteropServices;

namespace Globe3DLight
{
    public static class SizeInBytes<T>
    {
        public static readonly int Value = Marshal.SizeOf(typeof(T));
    }
}
