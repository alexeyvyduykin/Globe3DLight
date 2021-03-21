using System;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal static class ArraySizeInBytes
    {
        public static int Size<T>(T[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            return values.Length * SizeInBytes<T>.Value;
        }
    }

}
