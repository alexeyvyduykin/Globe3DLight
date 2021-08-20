#nullable enable

namespace TimeDataViewer.Spatial
{
    public struct OxySize
    {
        public static readonly OxySize Empty = new(0, 0);
        private readonly double _height;
        private readonly double _width;

        public OxySize(double width, double height)
        {
            _width = width;
            _height = height;
        }

        public double Height => _height;

        public double Width => _width;
    }
}
