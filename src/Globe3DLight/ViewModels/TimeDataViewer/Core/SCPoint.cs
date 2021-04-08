#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    public struct SCPoint
    {
        public static readonly SCPoint Empty = new SCPoint();

        private int x;
        private int y;

        public SCPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public SCPoint(SCSize sz)
        {
            this.x = sz.Width;
            this.y = sz.Height;
        }

        //public SCPoint(int dw)
        //{
        //   this.x = (short) LOWORD(dw);
        //   this.y = (short) HIWORD(dw);
        //}

        public bool IsEmpty
        {
            get
            {
                return x == 0 && y == 0;
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

        public static explicit operator SCSize(SCPoint p)
        {
            return new SCSize(p.X, p.Y);
        }

        public static SCPoint operator +(SCPoint pt, SCSize sz)
        {
            return Add(pt, sz);
        }

        public static SCPoint operator -(SCPoint pt, SCSize sz)
        {
            return Subtract(pt, sz);
        }

        public static bool operator ==(SCPoint left, SCPoint right)
        {
            return left.X == right.X && left.Y == right.Y;
        }

        public static bool operator !=(SCPoint left, SCPoint right)
        {
            return !(left == right);
        }

        public static SCPoint Add(SCPoint pt, SCSize sz)
        {
            return new SCPoint(pt.X + sz.Width, pt.Y + sz.Height);
        }

        public static SCPoint Subtract(SCPoint pt, SCSize sz)
        {
            return new SCPoint(pt.X - sz.Width, pt.Y - sz.Height);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SCPoint))
                return false;
            SCPoint comp = (SCPoint)obj;
            return comp.X == this.X && comp.Y == this.Y;
        }

        public override int GetHashCode()
        {
            return (int)(x.GetHashCode() ^ y.GetHashCode());
        }

        public void Offset(int dx, int dy)
        {
            X += dx;
            Y += dy;
        }

        public void Offset(SCPoint p)
        {
            Offset(p.X, p.Y);
        }
        public void OffsetNegative(SCPoint p)
        {
            Offset(-p.X, -p.Y);
        }

        public override string ToString()
        {
            return "{X=" + X.ToString(CultureInfo.CurrentCulture) + ",Y=" + Y.ToString(CultureInfo.CurrentCulture) + "}";
        }

        //private static int HIWORD(int n)
        //{
        //   return (n >> 16) & 0xffff;
        //}

        //private static int LOWORD(int n)
        //{
        //   return n & 0xffff;
        //}
    }

    internal class SCPointComparer : IEqualityComparer<SCPoint>
    {
        public bool Equals(SCPoint x, SCPoint y)
        {
            return x.X == y.X && x.Y == y.Y;
        }

        public int GetHashCode(SCPoint obj)
        {
            return obj.GetHashCode();
        }
    }

    public static class Extensions
    {
        public static System.Drawing.Point ToPoint(this SCPoint point)
        {
            return new System.Drawing.Point(point.X, point.Y);
        }
    }
}
