#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Globe3DLight.Spatial;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    // the size of coordinates   
    public struct SCSchedulerSize
    {
        public static readonly SCSchedulerSize Empty;

        private double xSize;
        private double ySize;

        public SCSchedulerSize(SCSchedulerSize size)
        {
            this.xSize = size.xSize;
            this.ySize = size.ySize;
        }

        public SCSchedulerSize(Point2 pt)
        {
            this.xSize = pt.X;
            this.ySize = pt.Y;
        }

        public SCSchedulerSize(double xSize, double ySize)
        {
            this.xSize = xSize;
            this.ySize = ySize;
        }

        public static SCSchedulerSize operator +(SCSchedulerSize sz1, SCSchedulerSize sz2)
        {
            return Add(sz1, sz2);
        }

        public static SCSchedulerSize operator -(SCSchedulerSize sz1, SCSchedulerSize sz2)
        {
            return Subtract(sz1, sz2);
        }

        public static bool operator ==(SCSchedulerSize sz1, SCSchedulerSize sz2)
        {
            return ((sz1.SizeX == sz2.SizeX) && (sz1.SizeY == sz2.SizeY));
        }

        public static bool operator !=(SCSchedulerSize sz1, SCSchedulerSize sz2)
        {
            return !(sz1 == sz2);
        }

        public static explicit operator Point2(SCSchedulerSize size)
        {
            return new Point2(size.SizeX, size.SizeY);
        }

        public bool IsEmpty
        {
            get
            {
                return ((this.xSize == 0d) && (this.ySize == 0d));
            }
        }

        public double SizeX
        {
            get
            {
                return this.xSize;
            }
            set
            {
                this.xSize = value;
            }
        }

        public double SizeY
        {
            get
            {
                return this.ySize;
            }
            set
            {
                this.ySize = value;
            }
        }

        public static SCSchedulerSize Add(SCSchedulerSize sz1, SCSchedulerSize sz2)
        {
            return new SCSchedulerSize(sz1.SizeX + sz2.SizeX, sz1.SizeY + sz2.SizeY);
        }

        public static SCSchedulerSize Subtract(SCSchedulerSize sz1, SCSchedulerSize sz2)
        {
            return new SCSchedulerSize(sz1.SizeX - sz2.SizeX, sz1.SizeY - sz2.SizeY);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SCSchedulerSize))
            {
                return false;
            }
            SCSchedulerSize ef = (SCSchedulerSize)obj;
            return (((ef.SizeX == this.SizeX) && (ef.SizeY == this.SizeY)) && ef.GetType().Equals(base.GetType()));
        }

        public override int GetHashCode()
        {
            if (this.IsEmpty)
            {
                return 0;
            }
            return (this.SizeX.GetHashCode() ^ this.SizeY.GetHashCode());
        }

        public Point2 ToPointLatLng()
        {
            return (Point2)this;
        }

        public override string ToString()
        {
            return ("{SizeX=" + this.SizeX.ToString(CultureInfo.CurrentCulture) + ", SizeY=" + this.SizeY.ToString(CultureInfo.CurrentCulture) + "}");
        }

        static SCSchedulerSize()
        {
            Empty = new SCSchedulerSize();
        }
    }
}
