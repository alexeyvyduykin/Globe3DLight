#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Globe3DLight.Spatial;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    // the rect  
    public struct SCRect
    {
        public static readonly SCRect Empty = new SCRect();

        private int x;
        private int y;
        private int width;
        private int height;

        public SCRect(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public SCRect(SCPoint location, SCSize size)
        {
            this.x = location.X;
            this.y = location.Y;
            this.width = size.Width;
            this.height = size.Height;
        }

        public SCRect(SCPoint point1, SCPoint point2)
        {
            this.x = point1.X;
            this.y = point1.Y;
            this.width = point2.X - point1.X;
            this.height = point2.Y - point1.Y;
        }

        public static SCRect FromLTRB(int left, int top, int right, int bottom)
        {
            return new SCRect(left,
                                 top,
                                 right - left,
                                 bottom - top);
        }

        public SCPoint Location
        {
            get
            {
                return new SCPoint(X, Y);
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public SCPoint RightBottom
        {
            get
            {
                return new SCPoint(Right, Bottom);
            }
        }

        public SCPoint RightTop
        {
            get
            {
                return new SCPoint(Right, Top);
            }
        }

        public SCPoint LeftBottom
        {
            get
            {
                return new SCPoint(Left, Bottom);
            }
        }

        public SCSize Size
        {
            get
            {
                return new SCSize(Width, Height);
            }
            set
            {
                this.Width = value.Width;
                this.Height = value.Height;
            }
        }

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        public int Left
        {
            get
            {
                return X;
            }
        }

        public int Top
        {
            get
            {
                return Y;
            }
        }

        public int Right
        {
            get
            {
                return X + Width;
            }
        }

        public int Bottom
        {
            get
            {
                return Y + Height;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return height == 0 && width == 0 && x == 0 && y == 0;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SCRect))
                return false;

            SCRect comp = (SCRect)obj;

            return (comp.X == this.X) &&
               (comp.Y == this.Y) &&
               (comp.Width == this.Width) &&
               (comp.Height == this.Height);
        }

        public static bool operator ==(SCRect left, SCRect right)
        {
            return (left.X == right.X
                       && left.Y == right.Y
                       && left.Width == right.Width
                       && left.Height == right.Height);
        }

        public static bool operator !=(SCRect left, SCRect right)
        {
            return !(left == right);
        }

        public bool Contains(long x, long y)
        {
            return this.X <= x &&
               x < this.X + this.Width &&
               this.Y <= y &&
               y < this.Y + this.Height;
        }

        public bool Contains(SCPoint pt)
        {
            return Contains(pt.X, pt.Y);
        }

        public bool Contains(SCRect rect)
        {
            return (this.X <= rect.X) &&
               ((rect.X + rect.Width) <= (this.X + this.Width)) &&
               (this.Y <= rect.Y) &&
               ((rect.Y + rect.Height) <= (this.Y + this.Height));
        }

        public override int GetHashCode()
        {
            if (this.IsEmpty)
            {
                return 0;
            }
            return (int)(((this.X ^ ((this.Y << 13) | (this.Y >> 0x13))) ^ ((this.Width << 0x1a) | (this.Width >> 6))) ^ ((this.Height << 7) | (this.Height >> 0x19)));
        }

        public void Inflate(int width, int height)
        {
            this.X -= width;
            this.Y -= height;
            this.Width += 2 * width;
            this.Height += 2 * height;
        }

        public void Inflate(SCSize size)
        {
            Inflate(size.Width, size.Height);
        }

        public static SCRect Inflate(SCRect rect, int x, int y)
        {
            SCRect r = rect;
            r.Inflate(x, y);
            return r;
        }

        public void Intersect(SCRect rect)
        {
            SCRect result = SCRect.Intersect(rect, this);

            this.X = result.X;
            this.Y = result.Y;
            this.Width = result.Width;
            this.Height = result.Height;
        }

        public static SCRect Intersect(SCRect a, SCRect b)
        {
            int x1 = Math.Max(a.X, b.X);
            int x2 = Math.Min(a.X + a.Width, b.X + b.Width);
            int y1 = Math.Max(a.Y, b.Y);
            int y2 = Math.Min(a.Y + a.Height, b.Y + b.Height);

            if (x2 >= x1
                   && y2 >= y1)
            {

                return new SCRect(x1, y1, x2 - x1, y2 - y1);
            }
            return SCRect.Empty;
        }

        public bool IntersectsWith(SCRect rect)
        {
            return (rect.X < this.X + this.Width) &&
               (this.X < (rect.X + rect.Width)) &&
               (rect.Y < this.Y + this.Height) &&
               (this.Y < rect.Y + rect.Height);
        }

        public static SCRect Union(SCRect a, SCRect b)
        {
            int x1 = Math.Min(a.X, b.X);
            int x2 = Math.Max(a.X + a.Width, b.X + b.Width);
            int y1 = Math.Min(a.Y, b.Y);
            int y2 = Math.Max(a.Y + a.Height, b.Y + b.Height);

            return new SCRect(x1, y1, x2 - x1, y2 - y1);
        }

        public void Offset(SCPoint pos)
        {
            Offset(pos.X, pos.Y);
        }

        public void OffsetNegative(SCPoint pos)
        {
            Offset(-pos.X, -pos.Y);
        }

        public void Offset(int x, int y)
        {
            this.X += x;
            this.Y += y;
        }

        public override string ToString()
        {
            return "{X=" + X.ToString(CultureInfo.CurrentCulture) + ",Y=" + Y.ToString(CultureInfo.CurrentCulture) +
               ",Width=" + Width.ToString(CultureInfo.CurrentCulture) +
               ",Height=" + Height.ToString(CultureInfo.CurrentCulture) + "}";
        }
    }

    public struct SCWindow
    {
        public static readonly SCWindow Empty = new SCWindow();

        private int x;
        private int y;
        private int width;
        private int height;

        public SCWindow(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public SCWindow(SCPoint location, SCSize size)
        {
            this.x = location.X;
            this.y = location.Y;
            this.width = size.Width;
            this.height = size.Height;
        }

        public SCSize Size
        {
            get
            {
                return new SCSize(Width, Height);
            }
            set
            {
                this.Width = value.Width;
                this.Height = value.Height;
            }
        }

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        public int Left
        {
            get
            {
                return x;
            }
        }

        public int Right
        {
            get
            {
                return x + width;
            }
        }

        public int Bottom
        {
            get
            {
                return y;
            }
        }

        public int Top
        {
            get
            {
                return y + height;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return height == 0 && width == 0 && x == 0 && y == 0;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SCWindow))
                return false;

            SCWindow comp = (SCWindow)obj;

            return (comp.X == this.X) &&
               (comp.Y == this.Y) &&
               (comp.Width == this.Width) &&
               (comp.Height == this.Height);
        }

        public static bool operator ==(SCWindow left, SCWindow right)
        {
            return (left.X == right.X
                       && left.Y == right.Y
                       && left.Width == right.Width
                       && left.Height == right.Height);
        }

        public static bool operator !=(SCWindow left, SCWindow right)
        {
            return !(left == right);
        }

        public bool Contains(int x, int y)
        {
            return this.X <= x && x < this.X + this.Width &&
                   this.Y <= y && y < this.Y + this.Height;
        }

        public bool Contains(SCPoint pt)
        {
            return Contains(pt.X, pt.Y);
        }

        public bool Contains(SCWindow rect)
        {
            return (this.X <= rect.X) && ((rect.X + rect.Width) <= (this.X + this.Width)) &&
                   (this.Y <= rect.Y) && ((rect.Y + rect.Height) <= (this.Y + this.Height));
        }

        public override int GetHashCode()
        {
            if (this.IsEmpty)
            {
                return 0;
            }
            return (int)(((this.X ^ ((this.Y << 13) | (this.Y >> 0x13))) ^ ((this.Width << 0x1a) | (this.Width >> 6))) ^ ((this.Height << 7) | (this.Height >> 0x19)));
        }

        public void Offset(SCPoint pos)
        {
            Offset(pos.X, pos.Y);
        }

        public void OffsetNegative(SCPoint pos)
        {
            Offset(-pos.X, -pos.Y);
        }

        public void Offset(int x, int y)
        {
            this.X += x;
            this.Y += y;
        }

        public override string ToString()
        {
            return "{X=" + X.ToString(CultureInfo.CurrentCulture) + ",Y=" + Y.ToString(CultureInfo.CurrentCulture) +
               ",Width=" + Width.ToString(CultureInfo.CurrentCulture) +
               ",Height=" + Height.ToString(CultureInfo.CurrentCulture) + "}";
        }
    }

    public struct SCViewport
    {
        public static readonly SCViewport Empty = new SCViewport();

        private double x;
        private double y;
        private double width;
        private double height;

        public SCViewport(double x, double y, double width, double height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public SCViewport(SCViewport viewport)
        {
            this.x = viewport.x;
            this.y = viewport.y;
            this.width = viewport.width;
            this.height = viewport.height;
        }

        public double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public double Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        public double Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        public double Left
        {
            get
            {
                return x;
            }
        }

        public double Right
        {
            get
            {
                return x + width;
            }
        }

        public double Bottom
        {
            get
            {
                return y;
            }
        }

        public double Top
        {
            get
            {
                return y + height;
            }
        }

        public Point2 P0
        {
            get
            {
                return new Point2(x, y);
            }
        }

        public Point2 P1
        {
            get
            {
                return new Point2(x + width, y + height);
            }
        }

        public double MiddleX
        {
            get
            {
                return x + width / 2.0;
            }
        }

        public double MiddleY
        {
            get
            {
                return y + height / 2.0;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return height == 0.0 && width == 0.0 && x == 0.0 && y == 0.0;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SCViewport))
                return false;

            SCViewport comp = (SCViewport)obj;

            return (comp.X == this.X) && (comp.Y == this.Y) &&
                   (comp.Width == this.Width) && (comp.Height == this.Height);
        }

        public static bool operator ==(SCViewport left, SCViewport right)
        {
            return (left.X == right.X && left.Y == right.Y &&
                    left.Width == right.Width && left.Height == right.Height);
        }

        public static bool operator !=(SCViewport left, SCViewport right)
        {
            return !(left == right);
        }

        public bool Contains(double x, double y)
        {
            return this.X <= x && x < this.X + this.Width &&
               this.Y <= y && y < this.Y + this.Height;
        }

        public bool Contains(SCViewport rect)
        {
            return (this.X <= rect.X) && ((rect.X + rect.Width) <= (this.X + this.Width)) &&
                   (this.Y <= rect.Y) && ((rect.Y + rect.Height) <= (this.Y + this.Height));
        }

        public override int GetHashCode()
        {
            if (this.IsEmpty)
            {
                return 0;
            }
            return (int)((((int)this.X ^ (((int)this.Y << 13) | ((int)this.Y >> 0x13))) ^ (((int)this.Width << 0x1a) | ((int)this.Width >> 6))) ^ (((int)this.Height << 7) | ((int)this.Height >> 0x19)));
        }

        public void Offset(double x, double y)
        {
            this.X += x;
            this.Y += y;
        }

        public override string ToString()
        {
            return "{X=" + X.ToString(CultureInfo.CurrentCulture) + ",Y=" + Y.ToString(CultureInfo.CurrentCulture) +
               ",Width=" + Width.ToString(CultureInfo.CurrentCulture) +
               ",Height=" + Height.ToString(CultureInfo.CurrentCulture) + "}";
        }
    }
}
