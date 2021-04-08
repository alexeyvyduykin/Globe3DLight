#nullable enable
using System;

namespace Globe3DLight.Spatial
{
    public struct RectD
    {
        public readonly double X;
        public readonly double Y;
        public readonly double Width;
        public readonly double Height;

        public RectD(double x, double y, double width, double height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public void Deconstruct(out double x, out double y, out double width, out double height)
        {
            x = this.X;
            y = this.Y;
            width = this.Width;
            height = this.Height;
        }

        public static RectD FromPoints(double x1, double y1, double x2, double y2, double dx = 0.0, double dy = 0.0)
        {
            double x = (double)Math.Min(x1 + dx, x2 + dx);
            double y = (double)Math.Min(y1 + dy, y2 + dy);
            double width = Math.Abs(Math.Max(x1 + dx, x2 + dx) - x);
            double height = Math.Abs(Math.Max(y1 + dy, y2 + dy) - y);
            return new RectD(x, y, width, height);
        }

        public static RectD FromPoints(Point2D tl, Point2D br, double dx = 0.0, double dy = 0.0)
        {
            return FromPoints(tl.X, tl.Y, br.X, br.Y, dx, dy);
        }

        public double Top
        {
            get { return Y; }
        }

        public double Left
        {
            get { return X; }
        }

        public double Bottom
        {
            get { return Y + Height; }
        }

        public double Right
        {
            get { return X + Width; }
        }

        public Point2D Center
        {
            get { return new Point2D(X + (Width / 2), Y + (Height / 2)); }
        }

        public Point2D TopLeft
        {
            get { return new Point2D(X, Y); }
        }

        public Point2D BottomRight
        {
            get { return new Point2D(X + Width, Y + Height); }
        }

        public bool Contains(Point2D point)
        {
            return (point.X >= X)
                && (point.X - Width <= X)
                && (point.Y >= Y)
                && (point.Y - Height <= Y);
        }

        public bool Contains(double x, double y)
        {
            return (x >= X)
                && (x - Width <= X)
                && (y >= Y)
                && (y - Height <= Y);
        }

        public bool IntersectsWith(RectD rect)
        {
            return (rect.Left <= Right)
                && (rect.Right >= Left)
                && (rect.Top <= Bottom)
                && (rect.Bottom >= Top);
        }
    }
}
